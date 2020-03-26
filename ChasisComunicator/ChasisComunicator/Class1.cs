using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChasisComunicator
{

    public class Startup
    {
        // This method is required by Katana:
        public void Configuration(Owin.IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();

            // Use the extension method provided by the WebApi.Owin library:
            app.UseWebApi(webApiConfiguration);
        }


        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            return config;
        }
    }


    public class KasaController : ApiController
    {
        public async Task<string> Get()
        {
            return await DitronExecuter.test();
        }
    }

    public class DitronExecuter
    {
        public static async Task<string> test()

        {

            return await Task.Factory.StartNew(() =>

            {
                Worker.startInfoMaker();
                while (Worker.workerThread.IsAlive)
                {
                    Thread.Sleep(100);
                }

                return Worker.s;




            });

        }

    }

    public class Worker
    {
        public static string s = "";
        public static Thread workerThread;
        public Worker()
        {

        }

        public static void startInfoMaker()
        {

            workerThread = new Thread(Work);
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();
        }
        public static void Work()
        {

            DriverInterface.connect("1");   //nr portes llogjike
            string serial = DriverInterface.GetFiscalNumber(); //seriali i kases
            DriverInterface.disconnect();
            s = serial;
        }

    }



    internal class ErrorChecker
    {
        public static bool IsAnError(int resultcode)
        {
            string errorcode = "";
            if (resultcode != 0)
            {
                switch (resultcode)
                {
                    case 1:
                        errorcode = "WEC_E_SONOTOPEN";
                        break;
                    case 2:
                        errorcode = "WEC_E_INIT";
                        break;
                    case 3:
                        errorcode = "WEC_E_PROGOPEN";
                        break;
                    case 4:
                        errorcode = "WEC_E_CONF";
                        break;
                    case 5:
                        errorcode = "WEC_E_ECRVERS";
                        break;
                    case 6:
                        errorcode = "WEC_E_NOIDLE";
                        break;
                    case 7:
                        errorcode = "WEC_E_PROGNOTOPEN";
                        break;
                    case 8:
                        errorcode = "WEC_E_NOREG";
                        break;
                    case 9:
                        errorcode = "WEC_E_NOEM";
                        break;
                    case 10:
                        errorcode = "WEC_E_FILEPAR";
                        break;
                    case 11:
                        errorcode = "WEC_E_FILEIN";
                        break;
                    case 12:
                        errorcode = "WEC_E_FILEERR";
                        break;
                    case 13:
                        errorcode = "WEC_E_FILELOG";
                        break;
                    case 14:
                        errorcode = "WEC_E_EXEC";
                        break;
                    case 15:
                        errorcode = "WEC_E_NOTNZ";
                        break;
                    case 16:
                        errorcode = "WEC_E_STEPS";
                        break;
                    case 17:
                        errorcode = "WEC_E_NOTSO";
                        break;
                    case 18:
                        errorcode = "WEC_E_SOOPEN";
                        break;
                    case 19:
                        errorcode = "WEC_E_SOBUSY";
                        break;
                    case 20:
                        errorcode = "WEC_E_PORT";
                        break;
                    case 21:
                        errorcode = "WEC_E_FAILURE";
                        break;
                    case 22:
                        errorcode = "WEC_E_OPENPAR";
                        break;
                    case 23:
                        errorcode = "WEC_E_SETP";
                        break;

                    case 256:
                        errorcode = "WEC_E_TRAD";
                        break;
                    case 256 + 1:
                        errorcode = "WEC_E_OPCOD_NF";
                        break;
                    case 256 + 2:
                        errorcode = "WEC_E_OPCOD_NU";
                        break;
                    case 256 + 3:
                        errorcode = "WEC_E_OPCOD_IL";
                        break;
                    case 256 + 4:
                        errorcode = "WEC_E_KEY_NF";
                        break;
                    case 256 + 5:
                        errorcode = "WEC_E_KEY_NU";
                        break;
                    case 256 + 6:
                        errorcode = "WEC_E_KEY_IL";
                        break;
                    case 256 + 7:
                        errorcode = "WEC_E_VAL_NOSIMB";
                        break;
                    case 256 + 8:
                        errorcode = "WEC_E_VAL_ALFA";
                        break;
                    case 256 + 9:
                        errorcode = "WEC_E_VAL_IL";
                        break;
                    case 256 + 10:
                        errorcode = "WEC_E_KEY_OBL";
                        break;
                    case 256 + 11:
                        errorcode = "WEC_E_EOL";
                        break;
                    case 256 + 12:
                        errorcode = "WEC_E_NOTSUP";
                        break;
                    case 256 + 13:
                        errorcode = "WEC_E_VEND_NOREP";
                        break;
                    case 256 + 14:
                        errorcode = "WEC_E_DATI_REP";
                        break;
                    case 256 + 15:
                        errorcode = "WEC_E_VAL_NFLEGGI";
                        break;
                    case 256 + 16:
                        errorcode = "WEC_E_NFLEGGI_NS";
                        break;
                    case 256 + 17:
                        errorcode = "WEC_E_GRUPPI";
                        break;
                    case 256 + 18:
                        errorcode = "WEC_E_TOOLONG";
                        break;
                    default:
                        errorcode = "<Unknown Error!>";
                        break;
                }
            }

            if (errorcode != "")
                return true;
            return false;

        }
    }
    /// <summary>
    /// Funksionet kryesore per krijimin dhe dergimin e komandave
    /// </summary>
    internal class DriverCommand
    {
        /// <summary>
        /// Komandat qe i dergohen kases
        /// </summary>
        #region Commands
        public const string BEGINPROG = "1";        //!< Komanda e fillim programim
        public const string ENDPROG = "2";          //!< Komanda e mbylljes se programimit
        public const string INP = "9";              //!< Kalim ne regjim P,
        public const string INFO = "71";            //!< Komanda per te marre info nga kasa
        public const string INFO_DISPLAY = "11";

        #endregion

        #region Symbols
        public const string YES = "2";
        public const string NO = "1";
        #endregion
        #region Fields
        private String command;
        private String retString;
        private int retCode = 0;
        #endregion

        #region Initialization
        public DriverCommand()
        {
            retCode = 0;
            retString = String.Empty;
            command = "";
        }
        #endregion

        #region SetCommand(s)
        public void SetStringCmd(string str)
        {
            retCode = 0;
            retString = String.Empty;
            command = str;
        }
        public void SetCommand(string commandCode)
        {
            retCode = 0;
            retString = String.Empty;
            command = "";
            command = "#" + commandCode + " ";
        }
        public void SetCommandBeginProg()
        {
            SetCommand(BEGINPROG);
        }
        public void SetCommandEndProg()
        {
            SetCommand(ENDPROG);
        }
        public void SetCommandConfirm()
        {
            SetCommand(INP);
            AddParameterConfirm();
        }
        public void SetCommandEnd()
        {
            SetCommand(INP);
            AddParameterEnd();
        }
        public void SetCommandSelect()
        {
            SetCommand(INP);
            AddParameterSelect();
        }
        #endregion

        #region Parameters
        public void AddParameter(string parameterCode, string parameterValue)
        {
            command += "#" + parameterCode + "=" + parameterValue + ",";
        }
        public void AddParameter(string parameterCode)
        {
            command += "#" + parameterCode + ",";
        }
        public void AddParameter(int parameterCode, string parameterValue)
        {
            AddParameter(parameterCode.ToString(), parameterValue);
        }
        public void AddParameter(int parameterCode)
        {
            AddParameter(parameterCode.ToString());
        }
        public void AddParameterConfirm()
        {
            AddParameter("4", "145");
        }
        public void AddParameterEnd()
        {
            AddParameter("4", "146");
        }
        public void AddParameterSelect()
        {
            AddParameter("4", "147");
        }
        #endregion

        #region Execute
        public bool Execute(AxCoEcrCom connection)
        {
            retString = "";
            if (command[command.Length - 1] == ',')
                command = command.Remove(command.Length - 1);

            retCode = connection.EcrCmd(command, ref retString);
            return (!ErrorChecker.IsAnError(retCode));
        }
        public bool Execute()
        {
            if (DriverInterface.connection != null)
                return Execute(DriverInterface.connection);
            return false;
        }
        #endregion

        #region Results
        public String GetResultString()
        {
            return retString;
        }
        public int GetResultCode()
        {
            return retCode;
        }
        #endregion
    }
    /// <summary>
    /// Klasa kryesore e me funksione te plota qe veprojne mbi kasen
    /// </summary>
    public class DriverInterface
    {
        #region Fields
        public static AxCOECRCOMLib.AxCoEcrCom connection;
        private static String retStr = String.Empty;
        private static string machineCode = String.Empty;
        #endregion


        #region Programming
        /// <summary>
        /// Funksion per lidhjen me kasen.
        /// Ky funksion therritet i pari.
        /// </summary>
        /// <param name="port">Porta logjike. E njejta porte logjike qe konfigurohet tek programi WinECRConf</param>
        /// <returns></returns>
        public static bool connect(string port)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriverInterface));

            connection = new AxCOECRCOMLib.AxCoEcrCom();
            ((System.ComponentModel.ISupportInitialize)(connection)).BeginInit();
            connection.Name = "connection1";
            connection.CreateControl();
            ((System.ComponentModel.ISupportInitialize)(connection)).EndInit();
            if (port != "")
            {
                int returnCode = 0;
                try
                {
                    returnCode = connection.Open("Port=" + port);

                    if (!ErrorChecker.IsAnError(returnCode))
                    {
                        connection.EventMask = 255;
                        connection.OutEditOptions = 0;
                        connection.EnableTradDC = true;
                        connection.OperatingMode = 0;

                        for (int i = 1; i <= 10; i++)
                        {
                            long iVC = connection.EcrConfVal(i * 0x10000 + 10);
                            iVC = connection.EcrConfVal(i * 0x10000 + 11);

                            iVC = connection.EcrConfVal(i * 0x10000 + 101);
                            iVC = connection.EcrConfVal(i * 0x10000 + 102);
                            iVC = connection.EcrConfVal(i * 0x10000 + 103);
                            iVC = connection.EcrConfVal(i * 0x10000 + 104);
                            iVC = connection.EcrConfVal(i * 0x10000 + 105);
                            iVC = connection.EcrConfVal(i * 0x10000 + 106);
                            iVC = connection.EcrConfVal(i * 0x10000 + 107);
                            iVC = connection.EcrConfVal(i * 0x10000 + 108);
                            iVC = connection.EcrConfVal(i * 0x10000 + 109);
                            iVC = connection.EcrConfVal(i * 0x10000 + 110);
                            iVC = connection.EcrConfVal(i * 0x10000 + 111);
                            iVC = connection.EcrConfVal(i * 0x10000 + 112);
                            iVC = connection.EcrConfVal(i * 0x10000 + 113);
                            iVC = connection.EcrConfVal(i * 0x10000 + 114);
                            iVC = connection.EcrConfVal(i * 0x10000 + 115);
                            iVC = connection.EcrConfVal(i * 0x10000 + 116);
                            iVC = connection.EcrConfVal(i * 0x10000 + 117);
                            iVC = connection.EcrConfVal(i * 0x10000 + 118);
                            iVC = connection.EcrConfVal(i * 0x10000 + 119);
                            iVC = connection.EcrConfVal(i * 0x10000 + 200);
                            iVC = connection.EcrConfVal(i * 0x10000 + 201);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Funksioni i shkeputjes nga kasa
        /// </summary>
        /// <returns>Kthen true nese shkeputja ishte e sukseshme dhe False nese jo.</returns>
        public static bool disconnect()
        {
            try
            {
                if (!ErrorChecker.IsAnError(connection.Close()))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Merr nr serial te kases. Psh. AE0100....
        /// </summary>
        /// <returns>Kthen nje string, seriali i kases</returns>
        public static string GetFiscalNumber()
        {
            DriverCommand cmd = new DriverCommand();

            cmd.SetCommand(DriverCommand.INFO);
            cmd.AddParameter("1", "1");
            if (!cmd.Execute(connection))
                return String.Empty;

            return cmd.GetResultString();
        }

        #region Commands
        private bool CommandFund()
        {

            DriverCommand cmd = new DriverCommand();
            cmd.SetCommand(DriverCommand.INP);
            cmd.AddParameterEnd();
            if (!cmd.Execute())
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Ekzekuto komanden Total
        /// </summary>
        /// <returns>True nese ekzekutohet mire.
        /// False nese nuk ekzekutohet mire.</returns>
        private bool CommandTotal()
        {
            DriverCommand cmd = new DriverCommand();
            cmd.SetCommand(DriverCommand.INP);
            cmd.AddParameterConfirm();
            if (!cmd.Execute())
            {
                return false;
            }
            return true;
        }
        #endregion

    }

}
