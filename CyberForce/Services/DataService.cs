using System;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Claims;
using CyberForce.Models;
using FluentFTP;
using Google.Protobuf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MySql.Data.MySqlClient;
using OpenPop.Mime;
using OpenPop.Pop3;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace CyberForce.Services
{
    public class DataService
    {
        public string ConnectionString { get; set; }

        public DataService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<SolarArray> GetSolarArrays()
        {
            List<SolarArray> list = new List<SolarArray>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from solar_arrays;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SolarArray()
                        {
                            ArrayId = Convert.ToInt32(reader["arrayID"]),
                            SolarStatus = Convert.ToInt32(reader["solarStatus"]),
                            OutputVoltage = Convert.ToInt32(reader["arrayVoltage"]),
                            OutputCurrent = Convert.ToInt32(reader["arrayCurrent"]),
                            Temperature = Convert.ToInt32(reader["arrayTemp"]),
                            TrackerTilt = Convert.ToInt32(reader["trackerTilt"]),
                            AzimuthAngle = Convert.ToInt32(reader["trackerAzimuth"]),
                        });
                    }
                }
            }
            return list;
        }

        //TODO POWER GENERATION VALUES

        public async Task<List<FtpListItem>> GetFtpListItems()
        {
            var client = new AsyncFtpClient("127.0.0.1");
            client.Config.ValidateAnyCertificate = true;
            await client.Connect();

            var items = await client.GetListing("/htdocs");

            return items.ToList<FtpListItem>();
        }

        public List<Message> GetAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            using (Pop3Client client = new Pop3Client())
            {
                client.Connect(hostname, port, useSsl);

                client.Authenticate(username, password);

                int messageCount = client.GetMessageCount();

                List<Message> allMessages = new List<Message>(messageCount);

                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }

                return allMessages;
            }
        }

        public (AuthUsers, bool) ValidateUser(string username, string password)
        {
            AuthUsers um = new();

            um.firstName = "Osman";
            um.lastName = "Yusof";
            um.emailAddress = "osman@yusof.com";
            um.sAMAccountName = "osmanyusof";
            if (username == "oyusof73@gmail.com")
            {
                um.userRole = "Admin";

            }
            else
            {
                um.userRole = "User";
            }
            um.domain = "yusof.local";

            return (um, true);
        }


        //    public (AuthUsers, bool) ValidateUser(string username, string password)
        //{
        //    string domain = "";
        //    List<string> UserADGroups = new List<string>();

        //    AuthUsers um = new AuthUsers();
        //    UserPrincipal up;


        //    Forest forest = Forest.GetCurrentForest();

        //    System.Diagnostics.Debug.WriteLine($"using domain : {domain}");

        //    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
        //    {
        //        try
        //        {
        //            System.Diagnostics.Debug.WriteLine($"connected server : {pc.ConnectedServer}");
        //            // Setup the user principal
        //            //up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, username);
        //            up = UserPrincipal.FindByIdentity(pc, IdentityType.UserPrincipalName, username);


        //            if (up == null) // The credentials did not validate
        //            {
        //                throw new Exception($"Invalid username or password for : {domain}\nSERVER : {pc.ConnectedServer}");
        //            }

        //            // Verify the account is not locked
        //            if (up.IsAccountLockedOut())
        //            {
        //                throw new Exception("User Account is Locked", new Exception("The account failed to logon " + up.BadLogonCount + " times."));
        //            }

        //            // Verify the account is not disabled
        //            if (up.Enabled == false || up.Enabled == null)
        //            {
        //                throw new Exception("User Account is Disabled");
        //            }

        //            if (pc.ValidateCredentials(username, password, ContextOptions.Negotiate))
        //            {
        //                var claims = new List<Claim>();
        //                claims.Add(new Claim(ClaimTypes.Name, username));
        //                claims.Add(new Claim(ClaimTypes.Hash, password));

        //                string role = "";

        //                PrincipalSearchResult<Principal>? groups = up.GetGroups();



        //                var groupList = groups.ToList().Select(x => x.ToString());

        //                if (groupList.Contains("WebApp Administrators"))
        //                {
        //                    role = "Admin";
        //                    claims.Add(new Claim(ClaimTypes.Role, role));
        //                }
        //                else if (groupList.Contains("WebApp Users"))
        //                {
        //                    role = "User";
        //                    claims.Add(new Claim(ClaimTypes.Role, role));
        //                }
        //                else
        //                {
        //                    role = "Unauthenticated";
        //                    claims.Add(new Claim(ClaimTypes.Role, role));
        //                }

        //                um.firstName = up.GivenName;
        //                um.lastName = up.Surname;
        //                um.emailAddress = up.EmailAddress;
        //                um.sAMAccountName = up.SamAccountName;
        //                um.userRole = role;
        //                um.domain = domain;

        //                //Create the claims pricipal
        //                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


        //                //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));

        //                return (um, true);
        //            }
        //            else // The credentials did not validate
        //            {
        //                throw new Exception($"Invalid credentials for {domain}.");
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            return (null, false);
        //        }
        //    }
        //}
    }
}

