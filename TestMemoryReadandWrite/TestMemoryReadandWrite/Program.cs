using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestMemoryReadandWrite
{
    class Program
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);


        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);


        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);


        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);


        public enum ProcessAccessType
        {
            PROCESS_TERMINATE = (0x0001),
            PROCESS_CREATE_THREAD = (0x0002),
            PROCESS_SET_SESSIONID = (0x0004),
            PROCESS_VM_OPERATION = (0x0008),
            PROCESS_VM_READ = (0x0010),
            PROCESS_VM_WRITE = (0x0020),
            PROCESS_DUP_HANDLE = (0x0040),
            PROCESS_CREATE_PROCESS = (0x0080),
            PROCESS_SET_QUOTA = (0x0100),
            PROCESS_SET_INFORMATION = (0x0200),
            PROCESS_QUERY_INFORMATION = (0x0400)
        }

        static void Main(string[] args)
        {
            //Festlegen aller benötigten Variablen
            System.Diagnostics.Process[] myProgrammInstances; //Array mit allen Prozessen des gewünschten Programmes
            ProcessAccessType myAccessFlags; //Die Zugrifssflags als Enummeration
            IntPtr handleProcess; //Hier wird der Handle welcher Als Rückgabewert der Funktion OpenProcess kommt gespeichert.

            //Initialisiere handleProcess auf einen Nullpointer
            handleProcess = IntPtr.Zero;

            //Fülle Das Array mit allen Verfügbaren Prozessen von Programmname
            myProgrammInstances = System.Diagnostics.Process.GetProcessesByName("NameDesProgrammes");

            //Setze Die Flags für lese/Schreibzugriff auf den Speicher
            myAccessFlags = ProcessAccessType.PROCESS_VM_READ
                            | ProcessAccessType.PROCESS_VM_WRITE
                            | ProcessAccessType.PROCESS_VM_OPERATION;

            //Hier wird letztendlich der Prozess geöffnet und als Handle Gespeichert
            handleProcess = OpenProcess((uint)myAccessFlags, 1, (uint)myProgrammInstances[0].Id);

            int closeHandleReturn;

            closeHandleReturn = CloseHandle(handleProcess);

            if (closeHandleReturn == 0)
            {
                Console.WriteLine("Handler not closed yet.");
                throw(new Exception().InnerException);
            }

        }
    }
}
