using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace WriteQueue
{
    public class QueueHandler
    {
        private static QueueHandler _instance;

        public static QueueHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new QueueHandler();
                return _instance;
            }
        }


        public bool CrearCola(string conn, string nombre, int size, int tiempo)
        {


            var c = NamespaceManager.CreateFromConnectionString(conn);

            if (c.QueueExists(nombre))
            {
                return false;
            }

            var cola = new QueueDescription(nombre);
            cola.MaxSizeInMegabytes = size;
            cola.DefaultMessageTimeToLive = new TimeSpan(0, 0, tiempo);

            
            try
            {
                c.CreateQueue(cola);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return true;
        }

        public void Enviar(string conn, string nombre, Dictionary<string, string> parametros, string texto)
        {
            var cl = QueueClient.CreateFromConnectionString(conn, nombre);
            var msg = new BrokeredMessage(texto);

            foreach (var key in parametros.Keys)
            {
                msg.Properties[key] = parametros[key];
            }

            cl.Send(msg);
        }
    }
}
