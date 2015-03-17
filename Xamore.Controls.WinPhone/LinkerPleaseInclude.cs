using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Xamore.Controls.WinPhone
{
    public static class LinkerPleaseInclude
    {
        private static DateTime initDate;
        public static void Init()
        {
            initDate = DateTime.UtcNow;
        }
    }
}