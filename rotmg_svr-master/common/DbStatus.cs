using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public enum LoginStatus
    {
        OK,
        AccountNotExists,
        InvalidCredentials
    }

    public enum RegisterStatus
    {
        OK,
        UsedName,
    }

    public enum CreateStatus
    {
        OK,
        ReachCharLimit
    }

    public static class StatusInfo
    {
        public static string GetInfo(this LoginStatus status)
        {
            switch (status)
            {
                case LoginStatus.InvalidCredentials:
                    return "Invalid Credentials";
                case LoginStatus.AccountNotExists:
                    return "No such account.";
                case LoginStatus.OK:
                    return "OK";
            }
            throw new ArgumentException("status");
        }

        public static string GetInfo(this RegisterStatus status)
        {
            switch (status)
            {
                case RegisterStatus.UsedName:
                    return "Duplicated Name";
                case RegisterStatus.OK:
                    return "OK";
            }
            throw new ArgumentException("status");
        }

        public static string GetInfo(this CreateStatus status)
        {
            switch (status)
            {
                case CreateStatus.ReachCharLimit:
                    return "Too many characters";
                case CreateStatus.OK:
                    return "OK";
            }
            throw new ArgumentException("status");
        }
    }
}
