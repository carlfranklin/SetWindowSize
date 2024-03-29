﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SetWindowSize
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);


        static void Main(string[] args)
        {
            Process[] processlist = Process.GetProcesses();
            var ProcessList = new List<WindowByLetter>();
            int counter = 96;
            const short SWP_NOMOVE = 0X2;
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    counter++;
                    var letter = (char)counter;
                    ProcessList.Add(new WindowByLetter { Process = process, Key = letter });
                    Console.WriteLine($"[{letter}] {process.MainWindowTitle}");
                }
            }

            Console.WriteLine();
            Console.Write("Select an open window by letter: ");
            var selection = Console.ReadKey();
            var wbl = (from x in ProcessList where x.Key == selection.KeyChar select x).FirstOrDefault();
            if (wbl != null)
            {
                var handle = wbl.Process.MainWindowHandle;
                Console.WriteLine();
                Console.Write("Select ENTER for 1080p or 4 for 4K: ");
                selection = Console.ReadKey();

                if (selection.Key == ConsoleKey.T || selection.Key == ConsoleKey.Enter)
                {
                    int x = (5120 - 1920) / 2;
                    int y = (1440 - 1080) / 2;
                    SetWindowPos(handle, 0, x, y, 1920, 1080, SWP_NOZORDER | SWP_SHOWWINDOW);
                }
                else if (selection.Key == ConsoleKey.D4 || selection.Key == ConsoleKey.NumPad4)
                {
                    int x = (5120 - 3840) / 2;
                    int y = (1440 - 2160) / 2;
                    SetWindowPos(handle, 0, 0, 0, 3840, 2160, SWP_NOZORDER | SWP_SHOWWINDOW);
                }
            }
        }

        public class WindowByLetter
        {
            public char Key { get; set; }
            public Process Process { get; set; }
        }
    }
}
