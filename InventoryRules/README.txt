This project use additional dictionary called VVFHOUSE.DIC that can be found in folder DexterityDictionary

This Project is to enforce business rules that can not be achieved using GP settings alone
1 (opt-out). Inventory Transaction Entry - selected user in ivruletargets(1) can post from transaction entry, others can not post (must use batch post)
2 (opt-in). Inventory Transaction Entry - selected user in ivruletargets(2) have default site = "STORE", batch = "ISSUE SEMENTARA" and adjustment type = 2, if doing adjustment type = 1, will be prompted
3 (opt-out). Shipment (Receiving Trx Entry) must use server Date and can not edit Receipt that has certain userdefined value, ivruletargets(3) will contain exception userid


As usual, I have several alternate form


This is the sanscript for Rule 3, to open POPInquiryReceivingEntry
'OpenWindow() of form POP_Inquiry_Receivings_Entry', 0, "GRN1400953", 2, 3, 2 ==> Posted Receipt Open PO
'OpenWindow() of form POP_Inquiry_Receivings_Entry', 0, "GRN1401157", 2, 4, 2 ==> Saved Receipt Open PO
'OpenWindow() of form POP_Inquiry_Receivings_Entry', 0, "GRN1400356", 2, 3, 2 ==> Posted Receipt History PO

public static DateTime GetNetworkTime()
        {
            const string ntpServer = "vvfi-dc01";
            var ntpData = new byte[48];
            ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }
		
		
public class GPUser
{
    public readonly static string DataBase = Dynamics.Globals.IntercompanyId.Value;
    public readonly static string UserID = Dynamics.Globals.UserId.Value;
    public readonly static string Password = Dynamics.Globals.SqlPassword.Value;
    public readonly static string DataSource = Dynamics.Globals.SqlDataSourceName.Value;
    public readonly static string ApplicationName = string.Format("{0}{1}", App.ProductName, "(gp)");
    public static SqlConnectionStringBuilder ConnectionString
    {
        get 
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = DataSource,
                UserID = UserID,
                Password = Password,
                ApplicationName = ApplicationName,
                InitialCatalog = DataBase
            };
        }

    }
    public readonly static short CompanyId = Dynamics.Globals.CompanyId.Value;
    public readonly static DateTime UserDate = Dynamics.Globals.UserDate.Value;
}


SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder(connectionString);
SqlConnection sqlConn = new SqlConnection();
if (sqlConn.State != ConnectionState.Open)
{
    GPConnection.Startup();
    var gpconn = new GPConnection();
    gpconn.Init(<Key1>, <Key2>);
    try
    {
        sqlConn.ConnectionString = string.Format("database={0}", cb.InitialCatalog);
        gpconn.LoginCompatibilityMode = false;
        gpconn.Connect(sqlConn, cb.DataSource, cb.UserID, cb.Password);
        if (gpconn.ReturnCode != 1)
            throw new AuthenticationException("Could not authenticate with the GP    credentials.");
    }
    catch (System.Runtime.InteropServices.SEHException)
    {
        throw new AuthenticationException("Could not authenticate with the GP credentials.");
    }
}