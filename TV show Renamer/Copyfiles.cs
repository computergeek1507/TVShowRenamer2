
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace TV_Show_Renamer
{

    public class FileCopiedEventArgs
    {
        public FileCopiedEventArgs(string file) { File = file; }

        public FileCopiedEventArgs(string file, Exception exception)
        {
            File = file;
            Error = exception;
        }

        public String File { get; private set; } // readonly  
        public Exception Error { get; private set; }
        public bool Cancel;
    }

    public class FileCopyCompleteEventArgs
    {
        public FileCopyCompleteEventArgs(bool cancel, Exception lastError) { Cancel = cancel; LastError = lastError; }
        public bool Cancel { get; private set; }
        public Exception LastError { get; private set; }
    }

    public class CopyFiles
    {

        // Variables
        private List<String> files = new List<String>();
        private readonly List<String> newFilenames = new List<String>();
        private Int32 _totalFiles = 0;
        private Int32 _totalFilesCopied = 0;
        private readonly String _destinationDir = "";
        private readonly String _sourceDir = "";
        private String _currentFilename;
        private Boolean _cancel = false;
        private Thread _copyThread;
        private const int ERROR_REQUEST_ABORTED = 1235;

        private bool _moveNotCopy = false;

        // Events
        public event FileCopyCompleteEventHandler CopyComplete;
        public event FileCopiedEventHandler FileCopied;

        // Delegates
        public delegate void FileCopiedEventHandler(object sender, FileCopiedEventArgs e);
        public delegate void FileCopyCompleteEventHandler(object sender, FileCopyCompleteEventArgs e);


        public CopyFiles(List<String> sourceFiles, List<String> destFiles, bool moveNotCopy)
        {
            if (sourceFiles.Count != destFiles.Count) throw new Exception("Array Length Mismatch");

            files = sourceFiles;
            _totalFiles = files.Count;
            newFilenames = destFiles;
            _moveNotCopy = moveNotCopy;


        }

        public delegate void CopyProgressHandlerDelegate(Int32 totalFiles, Int32 copiedFiles, Int64 totalBytes, Int64 copiedBytes, String currentFilename);
        public event CopyProgressHandlerDelegate ProgressEvent;

        // Methods
        private List<String> GetFiles(String sourceDir)
        {

            // Variables
            String[] fileEntries;
            String[] subdirEntries;

            //Add root files in this DIR to the list
            fileEntries = System.IO.Directory.GetFiles(sourceDir);
            List<String> foundFiles = fileEntries.ToList();

            //Loop the DIR's in the current DIR
            subdirEntries = System.IO.Directory.GetDirectories(sourceDir);
            foreach (string subdir in subdirEntries)
            {

                //Dont open Folder Redirects as this can end up in an infinate loop
                if ((System.IO.File.GetAttributes(subdir) &
                     System.IO.FileAttributes.ReparsePoint) !=
                     System.IO.FileAttributes.ReparsePoint)
                {
                    //Run recursivly to follow this DIR tree 
                    //adding all the files along the way
                    foundFiles.AddRange(GetFiles(subdir));
                }

            }

            return foundFiles;
        }

        private NativeMethods.CopyProgressResult CopyProgressHandler(Int64 total, Int64 transferred, Int64 streamSize, Int64 streamByteTrans, UInt32 dwStreamNumber, NativeMethods.CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            if (ProgressEvent != null)
                ProgressEvent(_totalFiles, _totalFilesCopied, total, transferred, _currentFilename);

            return _cancel ? NativeMethods.CopyProgressResult.PROGRESS_CANCEL : NativeMethods.CopyProgressResult.PROGRESS_CONTINUE;
        }

        public void CancelCopy()
        {
            _cancel = true;
        }

        private void Copyfiles()
        {
            try
            {
                Exception lastError = null;
                Int32 index = 0;

                //If we have been a sourceDIR then find all the files to copy
                if (!String.IsNullOrEmpty(_sourceDir))
                    files = GetFiles(_sourceDir);
                _totalFiles = files.Count;

                //Loop each file and copy it.
                foreach (String filename in files.ToArray())
                {
                    String tempFilepath;

                    //If we have a source directory, strip that off the filename
                    if (!String.IsNullOrEmpty(_sourceDir))
                    {
                        tempFilepath = filename;
                        tempFilepath = tempFilepath.Replace(_sourceDir, "").TrimStart(Path.DirectorySeparatorChar);
                        tempFilepath = System.IO.Path.Combine(_destinationDir, tempFilepath);
                    }
                    //otherwise strip off all the folder path
                    else
                    {
                        tempFilepath = String.IsNullOrEmpty(_destinationDir) ? newFilenames[index] : System.IO.Path.Combine(_destinationDir, newFilenames[index]);
                    }

                    //Save the new DIR path and check the DIR exsits,
                    //if it does not then create it so the files can copy
                    string tempDirPath = Path.GetDirectoryName(tempFilepath);
                    if (!System.IO.Directory.Exists(tempDirPath))
                        System.IO.Directory.CreateDirectory(tempDirPath);

                    //Have be been told to stop copying files
                    if (_cancel)
                        break;

                    //Set the file thats just about to get copied
                    _currentFilename = filename;

                    bool result = false;

                    if (_moveNotCopy)
                    {
                        result = NativeMethods.MoveFileEx(filename, tempFilepath, new NativeMethods.CopyProgressDelegate(this.CopyProgressHandler), IntPtr.Zero, ref _cancel, 0);
                    }
                    else
                    {
                        result = NativeMethods.CopyFileEx(filename, tempFilepath, new NativeMethods.CopyProgressDelegate(this.CopyProgressHandler), IntPtr.Zero, ref _cancel, 0);
                    }
                    //bool result = NativeMethods.CopyFileEx(filename, tempFilepath, new NativeMethods.CopyProgressDelegate(this.CopyProgressHandler), IntPtr.Zero, ref _cancel, 0);

                    _totalFilesCopied++;

                    if (!result)
                    {
                        if (Marshal.GetLastWin32Error() != ERROR_REQUEST_ABORTED)
                            lastError = new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                        else
                            _cancel = true;
                        OnFileCopied(filename, lastError);
                    }
                    else
                        OnFileCopied(filename);

                    index++;
                }
                OnCopyComplete(lastError);
            }
            catch (Exception ex)
            {
                OnCopyComplete(ex);
            }
        }

        protected void OnCopyComplete(Exception error)
        {
            if (CopyComplete != null)
            {
                CopyComplete(this, new FileCopyCompleteEventArgs(_cancel, error));
            }
        }

        protected void OnFileCopied(string fileName)
        {
            if (FileCopied != null)
            {
                FileCopiedEventArgs eventArgs = new FileCopiedEventArgs(fileName);
                FileCopied(this, eventArgs);
                _cancel = eventArgs.Cancel;
            }
        }

        protected void OnFileCopied(string fileName, Exception exception)
        {
            if (FileCopied == null) return;

            FileCopiedEventArgs eventArgs = new FileCopiedEventArgs(fileName, exception);
            eventArgs.Cancel = _cancel;
            FileCopied(this, eventArgs);
            _cancel = eventArgs.Cancel;
        }

        //Copy the files
        public void Copy()
        {
            _copyThread = new Thread(new ThreadStart(Copyfiles)) { Name = "CopyThread" };
            _copyThread.Start();
        }
    }

    internal static class NativeMethods
    {
        public const int FSCTL_SET_COMPRESSION = 0x9C040;
        public const short COMPRESSION_FORMAT_DEFAULT = 1;
        public const int HWND_BROADCAST = 0xFFFF;

        // Enums
        // These Enums are used for the windows CopyFileEx function
        [Flags]
        public enum CopyFileFlags : uint
        {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
        }
        public enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }
        public enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        public delegate CopyProgressResult CopyProgressDelegate(Int64 TotalFileSize, Int64 TotalBytesTransferred, Int64 StreamSize, Int64 StreamBytesTransferred, UInt32 dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CopyFileEx(string existingFileName, string newFileName, CopyProgressDelegate lpProgressRoutine, IntPtr lpData, ref bool pbCancel, CopyFileFlags dwCopyFlags);


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MoveFileEx(string existingFileName, string newFileName, CopyProgressDelegate lpProgressRoutine, IntPtr lpData, ref bool pbCancel, CopyFileFlags dwCopyFlags);
    }
}
