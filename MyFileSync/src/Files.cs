using MyFileSync.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyFileSync
{
    class Files
    {
        static public Task<List<FileDescription>> GetFileDescriptionsAsync(string folderName, SearchOption searchOption, IProgress<FileListUpReport> progress, CancellationToken cancellationToken)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderName);

            return Task.Run<List<FileDescription>>(() =>
            {
                List<FileDescription> list = new List<FileDescription>();

                try
                {
                    int total = 0;
                    int count = 0;

                    foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*", searchOption))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        total++;
                    }

                    progress.Report(new FileListUpReport(total, count));

                    foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*", searchOption))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        FileDescription fileDescription = new FileDescription(fileInfo);
                        list.Add(fileDescription);

                        count++;
                        if (count % 10 == 0) // 10ファイルごとに進歩を報告する
                        {
                            progress.Report(new FileListUpReport(total, count));
                        }
                    }

                    progress.Report(new FileListUpReport(total, total));
                } catch
                {
                    int total = -1;
                    int count = -1;

                    progress.Report(new FileListUpReport(total, count));
                }

                return list;
            });
        }

        static public Task<int> CountFilesAsync(string folder, SearchOption searchOption, CancellationToken cancellationToken)
        {
            return Task.Run<int>(() =>
            {
                int count = 0;

                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(folder);

                    foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*", searchOption))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        count++;
                    }
                }
                catch
                {
                }

                return count;
            });
        }

        static public async Task<FileComparisonResult> CompareFolders(string folder1, string folder2, SearchOption searchOption, IProgress<FileListUpReport> progress, CancellationToken cancellationToken)
        {
            int fileCount1 = await CountFilesAsync(folder1, searchOption, cancellationToken);
            int fileCount2 = await CountFilesAsync(folder2, searchOption, cancellationToken);

            Progress<FileListUpReport> progress1 = new Progress<FileListUpReport>((report) =>
            {
                progress.Report(new FileListUpReport(fileCount1 + fileCount2 + 1, report.Count));
            });

            Progress<FileListUpReport> progress2 = new Progress<FileListUpReport>((report) =>
            {
                progress.Report(new FileListUpReport(fileCount1 + fileCount2 + 1, report.Count + fileCount1));
            });

            List<FileDescription> fileDescriptions1 = await GetFileDescriptionsAsync(folder1, searchOption, progress1, cancellationToken);
            List<FileDescription> fileDescriptions2 = await GetFileDescriptionsAsync(folder2, searchOption, progress2, cancellationToken);

            FileComparisonResult fileComparisonResult = MakeFileComparisonResult(fileDescriptions1, fileDescriptions2);

            if (cancellationToken.IsCancellationRequested)
            {
                progress.Report(new FileListUpReport(-1, -1));
            }
            else
            {
                progress.Report(new FileListUpReport(fileCount1 + fileCount2 + 1,
                                                     fileCount1 + fileCount2 + 1));
            }

            return fileComparisonResult;
        }

        static private FileComparisonResult MakeFileComparisonResult(List<FileDescription> fileDescriptions1, List<FileDescription> fileDescriptions2)
        {
            List<FileDescription> aOnly = new List<FileDescription>();
            List<FileDescription> bOnly = new List<FileDescription>();

            List<IdenticalFile> identicals = new List<IdenticalFile>();

            foreach (FileDescription f in fileDescriptions1)
            {
                int index = fileDescriptions2.IndexOf(f);
                if (index < 0)
                {
                    // ａだけに存在
                    aOnly.Add(f);

                } else
                {
                    // 両方に存在

                    FileDescription aFileDescription = f;
                    FileDescription bFileDescription = fileDescriptions2[index];

                    IdenticalFile identical = new IdenticalFile(aFileDescription, bFileDescription);
                    identicals.Add(identical);

                    fileDescriptions2.RemoveAt(index);
                }
            }

            bOnly.AddRange(fileDescriptions2);

            FileComparisonResult fileComparisonResult = 
                                                 new FileComparisonResult(identicals, aOnly, bOnly);
            return fileComparisonResult;
        }
    }
}
