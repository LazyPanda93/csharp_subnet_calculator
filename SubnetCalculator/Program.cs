using System;

string userip;
string[] splits, ipOct;
string ipAdd;
int cidr;
bool isCidrValid;
int firstOct, secondOct, thirdOct, fourthOct, networkOct, broadOct;
int blockSize;
string netAdd, broadAdd;
int firstUsableOct, lastUsableOct;
string firstUsableHost, lastUsableHost;

//Keep asking for input until a valid IP address and CIDR are provided.
while (true)
{
    Console.Write("Enter the IP address with CIDR notation: ");
    userip = Console.ReadLine();

    if (!userip.Contains('/'))
    {
        Console.WriteLine("Invalid format. Use IP/CIDR, example: 192.168.1.10/24");
        continue;
    }

    splits = userip.Split('/');

    if (splits.Length != 2)
    {
        Console.WriteLine("Invalid format.");
        continue;
    }

    ipAdd = splits[0];


    //Validate that the IPv4 address is correctly formatted.
    if (!IsIpValid(ipAdd))
    {
        Console.WriteLine("IP must be valid.");
        continue;
    }

    //Convert the CIDR input string to an integer.
    isCidrValid = int.TryParse(splits[1], out cidr);


    if (!isCidrValid)
    {
        Console.WriteLine("CIDR must be a number.");
        continue;
    }

    if (cidr < 24 || cidr > 30)
    {
        Console.WriteLine("CIDR must be between 24 and 30.");
        continue;
    }

    break;
}


int host = UsableHost(cidr);
int nNetwork = NumberOfNetworks(cidr);

ipOct = ipAdd.Split(".");

firstOct = int.Parse(ipOct[0]);
secondOct = int.Parse(ipOct[1]);
thirdOct = int.Parse(ipOct[2]);
fourthOct = int.Parse(ipOct[3]);

blockSize = 256 / nNetwork;

//Checking for the network and broadcast addresses
networkOct = CalculateNetworkOctet(fourthOct, blockSize);
netAdd = BuildIpAdd(firstOct, secondOct,thirdOct, networkOct);

broadOct = CalculateBroadOctet(networkOct, blockSize);
broadAdd = BuildIpAdd(firstOct, secondOct, thirdOct, broadOct);

//Looking for the first and last usable host
firstUsableOct = networkOct + 1;
lastUsableOct = broadOct - 1;

firstUsableHost = BuildIpAdd(firstOct, secondOct, thirdOct,firstUsableOct);
lastUsableHost = BuildIpAdd(firstOct, secondOct, thirdOct, lastUsableOct);


Console.WriteLine("IP Address: " + ipAdd + "\nCIDR: " + cidr);

Console.WriteLine("Number of networks: " + nNetwork);
Console.WriteLine("Number of usable hosts: " + host);
Console.WriteLine("Network address: " + netAdd);
Console.WriteLine("Broadcast address: " + broadAdd);
Console.WriteLine("First usable host: " + firstUsableHost);
Console.WriteLine("Last usable host: " + lastUsableHost);
static int UsableHost(int cidr)
{

    return (int)Math.Pow(2, 32 - cidr) -2;
}

static int NumberOfNetworks(int cidr)
{
    return (int)Math.Pow(2, cidr - 24);
}

//Verify that the IPv4 address contains four valid octets.
static bool IsIpValid(string ip)
{
    string[] parts = ip.Split('.');

    if (parts.Length != 4)
    {
        return false;
    }

    foreach (string part in parts)
    {
        if (!int.TryParse(part, out int number))
        {
            return false;
        }

        if (number < 0 || number > 255)
        {
            return false;
        }
    }
    return true;
}

//Creating a method to multiuse for all network octet calculation
static int CalculateNetworkOctet(int ipOctet, int bSize)
{
    return (ipOctet / bSize) * bSize;
}

static int CalculateBroadOctet(int netwOctet, int bSize)
{
    return netwOctet + bSize - 1;
}

static string BuildIpAdd(int fOct, int sOct, int tOct, int lOct)
{
    return fOct + "." + sOct + "." + tOct + "." + lOct;
}