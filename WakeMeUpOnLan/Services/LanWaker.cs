using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WakeMeUpOnLan.Services {
    public static class LanWaker {

        public static async Task WakeOnLan( string macAddress ) {
            byte[] magicPacket = BuildMagicPacket( macAddress );
            foreach( NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces().Where( ( n ) =>
                n.NetworkInterfaceType != NetworkInterfaceType.Loopback && n.OperationalStatus == OperationalStatus.Up ) ) {
                IPInterfaceProperties iPInterfaceProperties = networkInterface.GetIPProperties();
                foreach( MulticastIPAddressInformation multicastIpAddressInformation in iPInterfaceProperties.MulticastAddresses ) {
                    IPAddress multicastIpAddress = multicastIpAddressInformation.Address;

                    if( multicastIpAddress.ToString().StartsWith( "ff02::1%", StringComparison.OrdinalIgnoreCase ) ) { // Ipv6: All hosts on LAN (with zone index)
                        UnicastIPAddressInformation unicastIPAddressInformation = iPInterfaceProperties.UnicastAddresses.FirstOrDefault( u => u.Address.AddressFamily == AddressFamily.InterNetworkV6 && !u.Address.IsIPv6LinkLocal );
                        if( unicastIPAddressInformation != null ) {
                            await SendWakeOnLan( unicastIPAddressInformation.Address, multicastIpAddress, magicPacket );
                            break;
                        }
                    } else if( multicastIpAddress.ToString().Equals( "224.0.0.1" ) ) { // Ipv4: All hosts on LAN

                        UnicastIPAddressInformation unicastIpAddressInformation = iPInterfaceProperties.UnicastAddresses.FirstOrDefault( u => u.Address.AddressFamily == AddressFamily.InterNetwork &&
                            !iPInterfaceProperties.GetIPv4Properties().IsAutomaticPrivateAddressingActive );
                        if( unicastIpAddressInformation != null ) {
                            await SendWakeOnLan( unicastIpAddressInformation.Address, multicastIpAddress, magicPacket );
                            break;
                        }
                    }
                }
            }

        }

        static byte[] BuildMagicPacket( string macAddress ) // MacAddress in any standard HEX format
        {
            macAddress = Regex.Replace( macAddress, "[: -]", "" );
            byte[] macBytes = new byte[6];
            for( int i = 0; i < 6; i++ ) {
                macBytes[i] = Convert.ToByte( macAddress.Substring( i * 2, 2 ), 16 );
            }

            using MemoryStream ms = new MemoryStream();
            using( BinaryWriter bw = new BinaryWriter( ms ) ) {
                //First 6 times 0xff
                for( int i = 0; i < 6; i++ ) {
                    bw.Write( (byte) 0xff );
                }
                // then 16 times MacAddress
                for( int i = 0; i < 16; i++ ) {
                    bw.Write( macBytes );
                }
            }
            return ms.ToArray(); // 102 bytes magic packet
        }

        static async Task SendWakeOnLan( IPAddress localIpAddress, IPAddress multicastIpAddress, byte[] magicPacket ) {
            using UdpClient client = new UdpClient( new IPEndPoint( localIpAddress, 0 ) );
            await client.SendAsync( magicPacket, magicPacket.Length, multicastIpAddress.ToString(), 9 );
        }

    }
}
