using System.Threading;

namespace kek
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new CecClient();
            if (client.Connect(10000))
            {
                client.SetActiveSource();
                Thread.Sleep(5000);
                client.Close();
            }
        }
    }
}
