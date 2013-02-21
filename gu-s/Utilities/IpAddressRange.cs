using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace gu_s.Utilities
{
    public class IpAddressRange
    {
        readonly AddressFamily addressFamily;
        readonly byte[] lowerBytes;
        readonly byte[] upperBytes;

        public IpAddressRange(string lowerString, string upperString)
        {
            // Assert that lower.AddressFamily == upper.AddressFamily
            var lower = IPAddress.Parse(lowerString);
            var upper = IPAddress.Parse(upperString);

           addressFamily = lower.AddressFamily;
           lowerBytes = lower.GetAddressBytes();
           upperBytes = upper.GetAddressBytes();
        }

        public bool IsInRange(string addressString)
        {
            var address = IPAddress.Parse(addressString);

            if (address.AddressFamily != addressFamily)
            {
                return false;
            }

            var addressBytes = address.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (var i = 0; i < lowerBytes.Length &&
                (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }
    }

}