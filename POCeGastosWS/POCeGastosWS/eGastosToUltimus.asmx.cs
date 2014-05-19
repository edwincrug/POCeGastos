using System;
using System.ComponentModel;
using System.Web.Services;
using eGastosEntity.Ultimus;
using System.Xml;
using System.Text;
using eGastosWS.WSeGastosPasteur;
using eGastosWS.ExpenseAccountServiceReference;
using eGastosWS.MissionOrderServiceReference;
using System.Globalization;
using System.Threading;
using eGastosWS.util;


namespace eGastosWS
{
    
    /// <summary>
    /// Summary description for eGastosToUltimus
    /// comentario de Marcio Nakamura
    /// </summary>
    [WebService(Namespace = "http://WSeGastos/Chatty/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class eGastosToUltimus : System.Web.Services.WebService
    {
        #region
        public WSeGastosPasteur.SchemaFile[] PasteurSchemaFile;
        public WSeGastosPharma.SchemaFile[] PharmaSchemaFile;
        public Entity.FilterData FilterData = new Entity.FilterData();
        public Entity.MasterEntity MasterEntity = new Entity.MasterEntity();
        public string msgError = "";
        #endregion
        private Mapeo mapeo = new Mapeo();
        private eGastosWS.Debug.Generate ge = new eGastosWS.Debug.Generate();
        private Entity.FilterData fd = new Entity.FilterData();

        [WebMethod]
        public int ExpensesAccountRequest(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        {
            String msgMO = generaExpenseAcc(me.UltRequest, me.UltExpenseAccount, me.UltExpenseAccountDetail, me.UltPAClient);
            if (string.IsNullOrEmpty(msgMO))
            {
                me.UltExpenseFlowVariables = mapeo.MapperData<eGastosWS.ExpenseAccountServiceReference.UltExpenseFlowVariables,
                    eGastosEntity.Ultimus.UltExpenseFlowVariables>(expAccClient.getUltExpenseFlowVariables());
                me.UltRequest = mapeo.MapperData<eGastosWS.ExpenseAccountServiceReference.UltRequest,
                    eGastosEntity.Ultimus.UltRequest>(expAccClient.getUltRequest());
                me.UltExpenseAccount = mapeo.MapperData<eGastosWS.ExpenseAccountServiceReference.UltExpenseAccount,
                    eGastosEntity.Ultimus.UltExpenseAccount>(expAccClient.getUltExpenseAccount());
                me.UltExpenseAccountDetail = mapeo.MapperDataList<eGastosWS.ExpenseAccountServiceReference.UltExpenseAccountDetail,
                    eGastosEntity.Ultimus.UltExpenseAccountDetail>(expAccClient.getUltExpenseAccountDetailList());
                me.UltPAClient = mapeo.MapperDataList<eGastosWS.ExpenseAccountServiceReference.UltPAClient,
                    eGastosEntity.Ultimus.UltPAClient>(expAccClient.getUltPAClientList());

                int nIncidente = 0;
                if (me.UltRequest.pasteur)
                {
                    incidentGeneratePasteur(me, fd, out msgError, out nIncidente, false);
                }
                else
                {
                    incidentGeneratePharma(me, fd, out msgError, out nIncidente, false);
                }
                error = msgError;
                return nIncidente;
            }
            error = msgMO;
            return 0;
        }

        [WebMethod]
        //public int MissionOrderRequest(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        public int MissionOrderRequest()
        {
            //// DEBUG
            string error = "";
            int nIncidente = 0;
            Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            me = ge.me1(ref fd);
            String msgMO = generaMissionOrder(me.UltRequest, me.UltMissionOrder, me.UltItinerary, me.UltHotel);
            if (string.IsNullOrEmpty(msgMO))
            {
                me.UltExpenseFlowVariables = mapeo.MapperData<eGastosWS.MissionOrderServiceReference.UltExpenseFlowVariables,
                    eGastosEntity.Ultimus.UltExpenseFlowVariables>(misOrdClient.getUltExpenseFlowVariables());
                me.UltRequest = mapeo.MapperData<eGastosWS.MissionOrderServiceReference.UltRequest,
                    eGastosEntity.Ultimus.UltRequest>(misOrdClient.getUltRequest());
                me.UltMissionOrder = mapeo.MapperData<eGastosWS.MissionOrderServiceReference.UltMissionOrder,
                    eGastosEntity.Ultimus.UltMissionOrder>(misOrdClient.getUltMissionOrder());
               
                me.UltItinerary = mapeo.MapperDataList<eGastosWS.MissionOrderServiceReference.UltItinerary,
                    eGastosEntity.Ultimus.UltItinerary>(misOrdClient.getUltItineraryList());
                me.UltHotel = mapeo.MapperDataList<eGastosWS.MissionOrderServiceReference.UltHotel,
                    eGastosEntity.Ultimus.UltHotel>(misOrdClient.getUltHotelList());

                if (me.UltRequest.pasteur)
                {
                    incidentGeneratePasteur(me, fd, out msgError, out nIncidente, false);
                }
                else
                {
                    incidentGeneratePharma(me, fd, out msgError, out nIncidente, false);
                }
                error = msgError;
                return nIncidente;
            }
            error = msgMO;
            return 0;            
        }

        [WebMethod]
        //public Entity.MasterEntity LoadMissionOrderApproval(Entity.FilterData fd)
        public Entity.MasterEntity LoadMissionOrderApproval()
        {
            //DEBUG
            Entity.FilterData fd = new Entity.FilterData();
            fd.ProcessName = "eGastos Pharma";
            fd.StepName = "Confirma Cotizacion";
            fd.UserLogin = "adultimus.local/marcio.nakamura";
            fd.IncidentNumber = 10;
            fd.isPasteur = false;

            string xmlstr = getUltimusXML(fd);

            //------------------------------------------------------------------

            populateEntity(fd, ref MasterEntity, xmlstr);


            return MasterEntity;
        }

        [WebMethod]
        public int SendMissionOrderApproval(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        //public int SendMissionOrderApproval()
        {
            ////DEBUG
            //Entity.FilterData fd = new Entity.FilterData();
            //Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            //me = ge.me1(ref fd);
            //// EndDebug
            ////------------------------------------------------------------------

            string xmlstr = getUltimusXML(fd);

            XmlDataDocument MOApprovalXML = new System.Xml.XmlDataDocument();
            XmlDocument oXmlDoc = new XmlDocument();
            MOApprovalXML.LoadXml(xmlstr.Replace("\"", "'"));

            string ProcessVersion = MOApprovalXML.ChildNodes.Item(1).Attributes["xmlns"].Value;
            int n = ProcessVersion.LastIndexOf("/");
            string ProcessVersionNumber = ProcessVersion.Substring(0, n).ToString() + "/Types";

            XmlNode XMLNodeGlobal = (MOApprovalXML.ChildNodes[1].ChildNodes[0]);
            XmlNode XMLNodeUltApprove = null;
            int nodeCont = XMLNodeGlobal.ChildNodes.Count;
            int nodeCountAH = 0, nodeAHLastIndex = 0, nodeAHFirstIndex = 0;

            string nodeName = "";
            XmlNode node = null;

            for (int h = 0; h < nodeCont; h++)
            {
                nodeName = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[h].Name;
                if (nodeName == "UltApprovalHistory")
                {
                    if (nodeAHFirstIndex == 0)
                        nodeAHFirstIndex = h;

                    nodeCountAH++;
                    nodeAHLastIndex = h;
                }
            }

            XmlElement xmlUltApprovalHistory = MOApprovalXML.CreateElement("UltApprovalHistory", ProcessVersionNumber);
            xmlUltApprovalHistory.InnerXml = "<stepName xmlns=\"http://processSchema.eGastos/\"></stepName><approverName xmlns=\"http://processSchema.eGastos/\"></approverName><approverLogin xmlns=\"http://processSchema.eGastos/\"></approverLogin><userEmail xmlns=\"http://processSchema.eGastos/\"></userEmail><approveDate xmlns=\"http://processSchema.eGastos/\"></approveDate><comments xmlns=\"http://processSchema.eGastos/\"></comments><approveStatus xmlns=\"http://processSchema.eGastos/\"></approveStatus>";
            MOApprovalXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltApprovalHistory, MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeAHLastIndex]);
            nodeCont = XMLNodeGlobal.ChildNodes.Count;//Counter Update
            nodeCountAH++;
            int a = 0;

            for (int i = nodeAHFirstIndex; (i < nodeCountAH + 1) && (a < me.UltApprovalHistory.Length); i++)
            {
                node = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];

                node.ChildNodes[0].InnerText = me.UltApprovalHistory[a].stepName;
                node.ChildNodes[1].InnerText = me.UltApprovalHistory[a].approverName;
                node.ChildNodes[2].InnerText = me.UltApprovalHistory[a].approverLogin;
                node.ChildNodes[3].InnerText = me.UltApprovalHistory[a].userEmail;
                node.ChildNodes[4].InnerText = ToXMLDateFormat(me.UltApprovalHistory[a].approveDate);
                node.ChildNodes[5].InnerText = me.UltApprovalHistory[a].comments;
                node.ChildNodes[6].InnerText = me.UltApprovalHistory[a].approveStatus;
                a++;
            }

            for (int i = 0; i < nodeCont; i++)
            {
                nodeName = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name;
                node = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];

                if (nodeName == "UltApprove")
                {
                    XMLNodeUltApprove = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                    node.ChildNodes[0].InnerText = me.UltApprove.approved.ToString().ToLower();
                    node.ChildNodes[1].InnerText = me.UltApprove.approverName;
                    node.ChildNodes[2].InnerText = me.UltApprove.approverLogin;
                    node.ChildNodes[3].InnerText = me.UltApprove.approverEmail;
                }
            }

            WSeGastosPharma.eGastos_Pharma ult_obj = new WSeGastosPharma.eGastos_Pharma();

            XmlNode XmlNodeCustom = (MOApprovalXML.ChildNodes[1].ChildNodes[0]);

            int intIncident = fd.IncidentNumber;
            string summary = "";
            string strError = "";
            ult_obj.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, MOApprovalXML.InnerXml, true, out strError);
            error = strError;
            return 0;
        }

        [WebMethod]
        public Entity.MasterEntity LoadExpensesAccountApproval(Entity.FilterData fd)
        //public Entity.MasterEntity LoadExpensesAccountApproval()
        {
            ////DEBUG
            //Entity.FilterData fd = new Entity.FilterData();
            //fd.ProcessName = "eGastos Pharma";
            //fd.StepName = "Confirma Cotizacion";
            //fd.UserLogin = "adultimus.local/marcio.nakamura";
            //fd.IncidentNumber = 10;
            //fd.isPasteur = false;
            //// /DEBUG

            string xmlstr = getUltimusXML(fd);
            //------------------------------------------------------------------
            populateEntity(fd, ref MasterEntity, xmlstr);
            return MasterEntity;
        }

        [WebMethod]
        public int SendExpensesAccountApproval(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        {
            ////DEBUG
            //Entity.FilterData fd = new Entity.FilterData();
            //Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            //me = ge.me1(ref fd);
            //// EndDebug
            ////------------------------------------------------------------------

            string xmlstr = getUltimusXML(fd);

            XmlDataDocument MOApprovalXML = new System.Xml.XmlDataDocument();
            XmlDocument oXmlDoc = new XmlDocument();
            MOApprovalXML.LoadXml(xmlstr.Replace("\"", "'"));

            string ProcessVersion = MOApprovalXML.ChildNodes.Item(1).Attributes["xmlns"].Value;
            int n = ProcessVersion.LastIndexOf("/");
            string ProcessVersionNumber = ProcessVersion.Substring(0, n).ToString() + "/Types";

            XmlNode XMLNodeGlobal = (MOApprovalXML.ChildNodes[1].ChildNodes[0]);
            XmlNode XMLNodeUltApprove = null;
            int nodeCont = XMLNodeGlobal.ChildNodes.Count;
            int nodeCountAH = 0, nodeAHLastIndex = 0, nodeAHFirstIndex = 0;

            string nodeName = "";
            XmlNode node = null;

            for (int h = 0; h < nodeCont; h++)
            {
                nodeName = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[h].Name;
                if (nodeName == "UltApprovalHistory")
                {
                    if (nodeAHFirstIndex == 0)
                        nodeAHFirstIndex = h;

                    nodeCountAH++;
                    nodeAHLastIndex = h;
                }
            }

            XmlElement xmlUltApprovalHistory = MOApprovalXML.CreateElement("UltApprovalHistory", ProcessVersionNumber);
            xmlUltApprovalHistory.InnerXml = "<stepName xmlns=\"http://processSchema.eGastos/\"></stepName><approverName xmlns=\"http://processSchema.eGastos/\"></approverName><approverLogin xmlns=\"http://processSchema.eGastos/\"></approverLogin><userEmail xmlns=\"http://processSchema.eGastos/\"></userEmail><approveDate xmlns=\"http://processSchema.eGastos/\"></approveDate><comments xmlns=\"http://processSchema.eGastos/\"></comments><approveStatus xmlns=\"http://processSchema.eGastos/\"></approveStatus>";
            MOApprovalXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltApprovalHistory, MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeAHLastIndex]);
            nodeCont = XMLNodeGlobal.ChildNodes.Count;
            nodeCountAH++;
            int a = 0;

            for (int i = nodeAHFirstIndex; (i < nodeCountAH + 1) && (a < me.UltApprovalHistory.Length); i++)
            {
                node = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];

                node.ChildNodes[0].InnerText = me.UltApprovalHistory[a].stepName;
                node.ChildNodes[1].InnerText = me.UltApprovalHistory[a].approverName;
                node.ChildNodes[2].InnerText = me.UltApprovalHistory[a].approverLogin;
                node.ChildNodes[3].InnerText = me.UltApprovalHistory[a].userEmail;
                node.ChildNodes[4].InnerText = ToXMLDateFormat(me.UltApprovalHistory[a].approveDate);
                node.ChildNodes[5].InnerText = me.UltApprovalHistory[a].comments;
                node.ChildNodes[6].InnerText = me.UltApprovalHistory[a].approveStatus;
                a++;
            }

            for (int i = 0; i < nodeCont; i++)
            {
                nodeName = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name;
                node = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];

                if (nodeName == "UltApprove")
                {
                    XMLNodeUltApprove = MOApprovalXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                    node.ChildNodes[0].InnerText = me.UltApprove.approved.ToString().ToLower();
                    node.ChildNodes[1].InnerText = me.UltApprove.approverName;
                    node.ChildNodes[2].InnerText = me.UltApprove.approverLogin;
                    node.ChildNodes[3].InnerText = me.UltApprove.approverEmail;
                }
            }

            WSeGastosPharma.eGastos_Pharma ult_obj = new WSeGastosPharma.eGastos_Pharma();

            XmlNode XmlNodeCustom = (MOApprovalXML.ChildNodes[1].ChildNodes[0]);

            int intIncident = fd.IncidentNumber;
            string summary = "";
            string strError = "";
            ult_obj.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, MOApprovalXML.InnerXml, true, out strError);
            error = strError;
            return 0;
        }

        private string getUltimusXML(Entity.FilterData fd)
        {
            string XMLSchemaData = "";

            if (fd.isPasteur)// Call Pasteur XML schema
            {
                WSeGastosPasteur.eGastos_Pasteur wsPasteur = new WSeGastosPasteur.eGastos_Pasteur();
                wsPasteur.GetTaskInformation(fd.UserLogin, fd.IncidentNumber, fd.StepName, out PasteurSchemaFile, out XMLSchemaData, out msgError);
            }
            else //Call Pharma XML Schema
            {
                WSeGastosPharma.eGastos_Pharma wsPHarma = new WSeGastosPharma.eGastos_Pharma();
                wsPHarma.GetTaskInformation(fd.UserLogin, fd.IncidentNumber, fd.StepName, out PharmaSchemaFile, out XMLSchemaData, out msgError);
            }
            return XMLSchemaData;
        }

        private string populateEntity(Entity.FilterData fd, ref Entity.MasterEntity me, string strXML)
        {
            XmlDataDocument ObjXML = new System.Xml.XmlDataDocument();
            XmlDocument oXmlDoc = new XmlDocument();

            ObjXML.LoadXml(strXML.Replace("\"", "'"));

            oXmlDoc.LoadXml(strXML);

            XmlNode XmlNodeGlobal = (ObjXML.ChildNodes[1].ChildNodes[0]);
            int NodeCount = XmlNodeGlobal.ChildNodes.Count;

            #region // ---- UltApprovalHistory ---- //
            int countUltApprovalHistory = ObjXML.GetElementsByTagName("UltApprovalHistory").Count;
            XmlNode[] NodeUltApprovalHistory = new XmlNode[countUltApprovalHistory];
            me.UltApprovalHistory = new UltApprovalHistory[countUltApprovalHistory];
            for (int _i = 0; _i < countUltApprovalHistory; _i++)
            {
                NodeUltApprovalHistory[_i] = ObjXML.GetElementsByTagName("UltApprovalHistory")[_i];
                me.UltApprovalHistory[_i] = new UltApprovalHistory();
                me.UltApprovalHistory[_i].approveDate = XmlConvert.ToDateTime(NodeUltApprovalHistory[_i]["approveDate"].InnerText.ToString());
                if (NodeUltApprovalHistory[_i]["approverLogin"] != null)
                {
                    me.UltApprovalHistory[_i].approverLogin = NodeUltApprovalHistory[_i]["approverLogin"].InnerText;
                }
                if (NodeUltApprovalHistory[_i]["approverName"] != null)
                {
                    me.UltApprovalHistory[_i].approverName = NodeUltApprovalHistory[_i]["approverName"].InnerText;
                }
                if (NodeUltApprovalHistory[_i]["approveStatus"] != null)
                {
                    me.UltApprovalHistory[_i].approveStatus = NodeUltApprovalHistory[_i]["approveStatus"].InnerText;
                }
                if (NodeUltApprovalHistory[_i]["comments"] != null)
                {
                    me.UltApprovalHistory[_i].comments = NodeUltApprovalHistory[_i]["comments"].InnerText;
                }
                if (NodeUltApprovalHistory[_i]["stepName"] != null)
                {
                    me.UltApprovalHistory[_i].stepName = NodeUltApprovalHistory[_i]["stepName"].InnerText;
                }
                if (NodeUltApprovalHistory[_i]["userEmail"] != null)
                {
                    me.UltApprovalHistory[_i].userEmail = NodeUltApprovalHistory[_i]["userEmail"].InnerText;
                }
            }
            #endregion // ---- UltApprovalHistory ---- //

            #region // ---- UltAttachments ---- //
            int countUltAttachments = ObjXML.GetElementsByTagName("UltAttachments").Count;
            XmlNode[] NodeUltAttachments = new XmlNode[countUltAttachments];
            me.UltAttachments = new UltAttachments[countUltAttachments];
            for (int _i = 0; _i < countUltAttachments; _i++)
            {
                NodeUltAttachments[_i] = ObjXML.GetElementsByTagName("UltAttachments")[_i];
                me.UltAttachments[_i] = new UltAttachments();
                me.UltAttachments[_i].Description = NodeUltAttachments[_i]["Description"].InnerText;
                me.UltAttachments[_i].FileDate = XmlConvert.ToDateTime(NodeUltAttachments[_i]["FileDate"].InnerText);
                me.UltAttachments[_i].FileName = NodeUltAttachments[_i]["FileName"].InnerText;
                me.UltAttachments[_i].FilePath = NodeUltAttachments[_i]["FilePath"].InnerText;
                me.UltAttachments[_i].FileType = NodeUltAttachments[_i]["FileType"].InnerText;
                me.UltAttachments[_i].Username = NodeUltAttachments[_i]["Username"].InnerText;
            }
            #endregion // ---- UltAttachments ---- //

            #region // ---- UltExpenseAccountDetail ---- //
            int countUltExpenseAccountDetail = ObjXML.GetElementsByTagName("UltExpenseAccountDetail").Count;
            XmlNode[] NodeUltExpenseAccountDetail = new XmlNode[countUltExpenseAccountDetail];
            me.UltExpenseAccountDetail = new eGastosEntity.Ultimus.UltExpenseAccountDetail[countUltExpenseAccountDetail];
            for (int _i = 0; _i < countUltExpenseAccountDetail; _i++)
            {
                NodeUltExpenseAccountDetail[_i] = ObjXML.GetElementsByTagName("UltExpenseAccountDetail")[_i];
                me.UltExpenseAccountDetail[_i] = new eGastosEntity.Ultimus.UltExpenseAccountDetail();


                if (NodeUltExpenseAccountDetail[_i]["accountName"] != null)
                    me.UltExpenseAccountDetail[_i].accountName = NodeUltExpenseAccountDetail[_i]["accountName"].InnerText;
                if (NodeUltExpenseAccountDetail[_i]["amount"] != null)
                    me.UltExpenseAccountDetail[_i].amount = XmlConvert.ToDouble(NodeUltExpenseAccountDetail[_i]["amount"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["discount"] != null)
                    me.UltExpenseAccountDetail[_i].discount = XmlConvert.ToDouble(NodeUltExpenseAccountDetail[_i]["discount"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["expenseDate"] != null)
                    me.UltExpenseAccountDetail[_i].expenseDate = XmlConvert.ToDateTime(NodeUltExpenseAccountDetail[_i]["expenseDate"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["hasPAClient"] != null)
                    me.UltExpenseAccountDetail[_i].hasPAClient = XmlConvert.ToBoolean(NodeUltExpenseAccountDetail[_i]["hasPAClient"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["healthProfessional"] != null)
                    me.UltExpenseAccountDetail[_i].healthProfessional = XmlConvert.ToBoolean(NodeUltExpenseAccountDetail[_i]["healthProfessional"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["idAccount"] != null)
                    me.UltExpenseAccountDetail[_i].idAccount = XmlConvert.ToInt32(NodeUltExpenseAccountDetail[_i]["idAccount"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["idExpenseAccount"] != null)
                    me.UltExpenseAccountDetail[_i].idExpenseAccount = XmlConvert.ToInt32(NodeUltExpenseAccountDetail[_i]["idExpenseAccount"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["idExpenseAccountDetail"] != null)
                    me.UltExpenseAccountDetail[_i].idExpenseAccountDetail = XmlConvert.ToInt32(NodeUltExpenseAccountDetail[_i]["idExpenseAccountDetail"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["invoiceNumber"] != null)
                    me.UltExpenseAccountDetail[_i].invoiceNumber = NodeUltExpenseAccountDetail[_i]["invoiceNumber"].InnerText;
                if (NodeUltExpenseAccountDetail[_i]["IVA"] != null)
                    me.UltExpenseAccountDetail[_i].IVA = XmlConvert.ToDouble(NodeUltExpenseAccountDetail[_i]["IVA"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["IVATypeId"] != null)
                    me.UltExpenseAccountDetail[_i].IVATypeId = NodeUltExpenseAccountDetail[_i]["IVATypeId"].InnerText;
                if (NodeUltExpenseAccountDetail[_i]["IVATypeName"] != null)
                    me.UltExpenseAccountDetail[_i].IVATypeName = NodeUltExpenseAccountDetail[_i]["IVATypeName"].InnerText;
                if (NodeUltExpenseAccountDetail[_i]["numberOfDiners"] != null)
                    me.UltExpenseAccountDetail[_i].numberOfDiners = XmlConvert.ToInt32(NodeUltExpenseAccountDetail[_i]["numberOfDiners"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["observationId"] != null)
                    me.UltExpenseAccountDetail[_i].observationId = XmlConvert.ToInt32(NodeUltExpenseAccountDetail[_i]["observationId"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["observationName"] != null)
                    me.UltExpenseAccountDetail[_i].observationName = NodeUltExpenseAccountDetail[_i]["observationName"].InnerText;
                if (NodeUltExpenseAccountDetail[_i]["place"] != null)
                    me.UltExpenseAccountDetail[_i].place = NodeUltExpenseAccountDetail[_i]["place"].InnerText;
                if (NodeUltExpenseAccountDetail[_i]["status"] != null)
                    me.UltExpenseAccountDetail[_i].status = XmlConvert.ToBoolean(NodeUltExpenseAccountDetail[_i]["status"].InnerText);
                if (NodeUltExpenseAccountDetail[_i]["total"] != null)
                    me.UltExpenseAccountDetail[_i].total = XmlConvert.ToDouble(NodeUltExpenseAccountDetail[_i]["total"].InnerText);

            }
            #endregion // ---- UltExpenseAccountDetail ---- //

            #region // ---- UltHotel ---- //
            int countUltHotel = ObjXML.GetElementsByTagName("UltHotel").Count;
            XmlNode[] NodeUltHotel = new XmlNode[countUltHotel];
            me.UltHotel = new eGastosEntity.Ultimus.UltHotel[countUltHotel];
            for (int _i = 0; _i < countUltHotel; _i++)
            {
                NodeUltHotel[_i] = ObjXML.GetElementsByTagName("UltHotel")[_i];
                me.UltHotel[_i] = new eGastosEntity.Ultimus.UltHotel();

                if (NodeUltHotel[_i]["address"] != null)
                    me.UltHotel[_i].address = NodeUltHotel[_i]["address"].InnerText;
                if (NodeUltHotel[_i]["checkInDate"] != null)
                    me.UltHotel[_i].checkInDate = XmlConvert.ToDateTime(NodeUltHotel[_i]["checkInDate"].InnerText);
                if (NodeUltHotel[_i]["checkoutDate"] != null)
                    me.UltHotel[_i].checkoutDate = XmlConvert.ToDateTime(NodeUltHotel[_i]["checkoutDate"].InnerText);
                if (NodeUltHotel[_i]["city"] != null)
                    me.UltHotel[_i].city = NodeUltHotel[_i]["city"].InnerText;
                if (NodeUltHotel[_i]["country"] != null)
                    me.UltHotel[_i].country = NodeUltHotel[_i]["country"].InnerText;
                if (NodeUltHotel[_i]["hotelName"] != null)
                    me.UltHotel[_i].hotelName = NodeUltHotel[_i]["hotelName"].InnerText;
                if (NodeUltHotel[_i]["hotelTax"] != null)
                    me.UltHotel[_i].hotelTax = XmlConvert.ToDouble(NodeUltHotel[_i]["hotelTax"].InnerText);
                if (NodeUltHotel[_i]["idConsecutive"] != null)
                    me.UltHotel[_i].idConsecutive = XmlConvert.ToInt32(NodeUltHotel[_i]["idConsecutive"].InnerText);
                if (NodeUltHotel[_i]["idHotel"] != null)
                    me.UltHotel[_i].idHotel = XmlConvert.ToInt32(NodeUltHotel[_i]["idHotel"].InnerText);
                if (NodeUltHotel[_i]["idLegerAccount"] != null)
                    me.UltHotel[_i].idLegerAccount = XmlConvert.ToInt32(NodeUltHotel[_i]["idLegerAccount"].InnerText);
                if (NodeUltHotel[_i]["idMissionOrder"] != null)
                    me.UltHotel[_i].idMissionOrder = XmlConvert.ToInt32(NodeUltHotel[_i]["idMissionOrder"].InnerText);
                if (NodeUltHotel[_i]["idRated"] != null)
                    me.UltHotel[_i].idRated = XmlConvert.ToInt32(NodeUltHotel[_i]["idRated"].InnerText);
                if (NodeUltHotel[_i]["IVA"] != null)
                    me.UltHotel[_i].IVA = XmlConvert.ToDouble(NodeUltHotel[_i]["IVA"].InnerText);
                if (NodeUltHotel[_i]["lineStatus"] != null)
                    me.UltHotel[_i].lineStatus = XmlConvert.ToInt32(NodeUltHotel[_i]["lineStatus"].InnerText);
                if (NodeUltHotel[_i]["lineStatusName"] != null)
                    me.UltHotel[_i].lineStatusName = NodeUltHotel[_i]["lineStatusName"].InnerText;
                if (NodeUltHotel[_i]["nameLegerAccount"] != null)
                    me.UltHotel[_i].nameLegerAccount = NodeUltHotel[_i]["nameLegerAccount"].InnerText;
                if (NodeUltHotel[_i]["observations"] != null)
                    me.UltHotel[_i].observations = NodeUltHotel[_i]["observations"].InnerText;
                if (NodeUltHotel[_i]["otherTaxes"] != null)
                    me.UltHotel[_i].otherTaxes = XmlConvert.ToDouble(NodeUltHotel[_i]["otherTaxes"].InnerText);
                if (NodeUltHotel[_i]["quotedRate"] != null)
                    me.UltHotel[_i].quotedRate = XmlConvert.ToDouble(NodeUltHotel[_i]["quotedRate"].InnerText);
                if (NodeUltHotel[_i]["realRate"] != null)
                    me.UltHotel[_i].realRate = XmlConvert.ToDouble(NodeUltHotel[_i]["realRate"].InnerText);
                if (NodeUltHotel[_i]["reservation"] != null)
                    me.UltHotel[_i].reservation = NodeUltHotel[_i]["reservation"].InnerText;
                if (NodeUltHotel[_i]["status"] != null)
                    me.UltHotel[_i].status = XmlConvert.ToBoolean(NodeUltHotel[_i]["status"].InnerText);
                if (NodeUltHotel[_i]["telephone"] != null)
                    me.UltHotel[_i].telephone = NodeUltHotel[_i]["telephone"].InnerText;

            }
            #endregion // ---- UltHotel ---- //

            #region// ---- UltItinerary ---- //
            int countUltItinerary = ObjXML.GetElementsByTagName("UltItinerary").Count;
            XmlNode[] NodeUltItinerary = new XmlNode[countUltItinerary];
            me.UltItinerary = new eGastosEntity.Ultimus.UltItinerary[countUltItinerary];
            for (int _i = 0; _i < countUltItinerary; _i++)
            {
                NodeUltItinerary[_i] = ObjXML.GetElementsByTagName("UltItinerary")[_i];
                me.UltItinerary[_i] = new eGastosEntity.Ultimus.UltItinerary();
                if (NodeUltItinerary[_i]["arrivalCity"] != null)
                    me.UltItinerary[_i].arrivalCity = NodeUltItinerary[_i]["arrivalCity"].InnerText;
                if (NodeUltItinerary[_i]["arrivalCountry"] != null)
                    me.UltItinerary[_i].arrivalCountry = NodeUltItinerary[_i]["arrivalCountry"].InnerText;
                if (NodeUltItinerary[_i]["arrivalDate"] != null && !string.IsNullOrEmpty(NodeUltItinerary[_i]["arrivalDate"].InnerText))
                    me.UltItinerary[_i].arrivalDate = XmlConvert.ToDateTime(NodeUltItinerary[_i]["arrivalDate"].InnerText.ToString());
                if (NodeUltItinerary[_i]["departureCity"] != null)
                    me.UltItinerary[_i].departureCity = NodeUltItinerary[_i]["departureCity"].InnerText;
                if (NodeUltItinerary[_i]["departureCountry"] != null)
                    me.UltItinerary[_i].departureCountry = NodeUltItinerary[_i]["departureCountry"].InnerText;
                if (NodeUltItinerary[_i]["departureDate"] != null && !string.IsNullOrEmpty(NodeUltItinerary[_i]["departureDate"].InnerText))
                    me.UltItinerary[_i].departureDate = XmlConvert.ToDateTime(NodeUltItinerary[_i]["departureDate"].InnerText.ToString());
                if (NodeUltItinerary[_i]["departureHour"] != null)
                    me.UltItinerary[_i].departureHour = NodeUltItinerary[_i]["departureHour"].InnerText;
                if (NodeUltItinerary[_i]["idConsecutive"] != null)
                    me.UltItinerary[_i].idConsecutive = XmlConvert.ToInt32(NodeUltItinerary[_i]["idConsecutive"].InnerText.ToString());
                if (NodeUltItinerary[_i]["idItinerary"] != null)
                    me.UltItinerary[_i].idItinerary = XmlConvert.ToInt32(NodeUltItinerary[_i]["idItinerary"].InnerText.ToString());
                if (NodeUltItinerary[_i]["idLedgerAccount"] != null)
                    me.UltItinerary[_i].idLedgerAccount = XmlConvert.ToInt32(NodeUltItinerary[_i]["idLedgerAccount"].InnerText.ToString());
                if (NodeUltItinerary[_i]["idMissionOrder"] != null)
                    me.UltItinerary[_i].idMissionOrder = XmlConvert.ToInt32(NodeUltItinerary[_i]["idMissionOrder"].InnerText.ToString());
                if (NodeUltItinerary[_i]["nameLedgerAccount"] != null)
                    me.UltItinerary[_i].nameLedgerAccount = NodeUltItinerary[_i]["nameLedgerAccount"].InnerText;
                if (NodeUltItinerary[_i]["nameTravelType"] != null)
                    me.UltItinerary[_i].nameTravelType = NodeUltItinerary[_i]["nameTravelType"].InnerText;
                if (NodeUltItinerary[_i]["observations"] != null)
                    me.UltItinerary[_i].observations = NodeUltItinerary[_i]["observations"].InnerText;
                if (NodeUltItinerary[_i]["returnHour"] != null)
                    me.UltItinerary[_i].returnHour = NodeUltItinerary[_i]["returnHour"].InnerText;
                if (NodeUltItinerary[_i]["status"] != null)
                    me.UltItinerary[_i].status = XmlConvert.ToBoolean(NodeUltItinerary[_i]["status"].InnerText);
                if (NodeUltItinerary[_i]["travelType"] != null)
                    me.UltItinerary[_i].travelType = XmlConvert.ToInt32(NodeUltItinerary[_i]["travelType"].InnerText.ToString());
            }
            #endregion// ---- UltItinerary ---- //

            #region// ---- UltItineraryOptions ---- //
            int countUltItineraryOptions = ObjXML.GetElementsByTagName("UltItineraryOptions").Count;
            XmlNode[] NodeUltItineraryOptions = new XmlNode[countUltItineraryOptions];
            me.UltItineraryOptions = new UltItineraryOptions[countUltItineraryOptions];
            for (int _i = 0; _i < countUltItineraryOptions; _i++)
            {
                NodeUltItineraryOptions[_i] = ObjXML.GetElementsByTagName("UltItineraryOptions")[_i];
                me.UltItineraryOptions[_i] = new UltItineraryOptions();
                if (NodeUltItineraryOptions[_i]["confirmed"] != null)
                    me.UltItineraryOptions[_i].confirmed = XmlConvert.ToBoolean(NodeUltItineraryOptions[_i]["confirmed"].InnerText.ToString());
                if (NodeUltItineraryOptions[_i]["idItineraryOption"] != null)
                    me.UltItineraryOptions[_i].idItineraryOption = XmlConvert.ToInt32(NodeUltItineraryOptions[_i]["idItineraryOption"].InnerText.ToString());
                if (NodeUltItineraryOptions[_i]["idMissionOrder"] != null)
                    me.UltItineraryOptions[_i].idMissionOrder = XmlConvert.ToInt32(NodeUltItineraryOptions[_i]["idMissionOrder"].InnerText.ToString());
                if (NodeUltItineraryOptions[_i]["idRate"] != null)
                    me.UltItineraryOptions[_i].idRate = XmlConvert.ToInt32(NodeUltItineraryOptions[_i]["idRate"].InnerText.ToString());
                if (NodeUltItineraryOptions[_i]["lastDayPurchase"] != null && !string.IsNullOrEmpty(NodeUltItineraryOptions[_i]["lastDayPurchase"].InnerText))
                    me.UltItineraryOptions[_i].lastDayPurchase = XmlConvert.ToDateTime(NodeUltItineraryOptions[_i]["lastDayPurchase"].InnerText.ToString());
                if (NodeUltItineraryOptions[_i]["observations"] != null)
                    me.UltItineraryOptions[_i].observations = NodeUltItineraryOptions[_i]["observations"].InnerText;
                if (NodeUltItineraryOptions[_i]["quoteRate"] != null)
                    me.UltItineraryOptions[_i].quoteRate = XmlConvert.ToDouble(NodeUltItineraryOptions[_i]["quoteRate"].InnerText.ToString());
            }
            #endregion// ---- UltItineraryOptions ---- //

            #region// ---- UltItineraryOptionsDetail ---- //
            int countUltItineraryOptionsDetail = ObjXML.GetElementsByTagName("UltItineraryOptionsDetail ").Count;
            XmlNode[] NodeUltItineraryOptionsDetail = new XmlNode[countUltItineraryOptionsDetail];
            me.UltItineraryOptionsDetail = new UltItineraryOptionsDetail[countUltItineraryOptionsDetail];
            for (int _i = 0; _i < countUltItineraryOptionsDetail; _i++)
            {
                NodeUltItineraryOptionsDetail[_i] = ObjXML.GetElementsByTagName("UltItineraryOptionsDetail ")[_i];
                me.UltItineraryOptionsDetail[_i] = new UltItineraryOptionsDetail();
                if (NodeUltItineraryOptionsDetail[_i]["airlineFlight"] != null)
                    me.UltItineraryOptionsDetail[_i].airlineFlight = NodeUltItineraryOptionsDetail[_i]["airlineFlight"].InnerText;
                if (NodeUltItineraryOptionsDetail[_i]["arrival"] != null)
                    me.UltItineraryOptionsDetail[_i].arrival = NodeUltItineraryOptionsDetail[_i]["arrival"].InnerText;
                if (NodeUltItineraryOptionsDetail[_i]["arrivalDate"] != null && !string.IsNullOrEmpty(NodeUltItineraryOptionsDetail[_i]["arrivalDate"].InnerText))
                    me.UltItineraryOptionsDetail[_i].arrivalDate = XmlConvert.ToDateTime(NodeUltItineraryOptionsDetail[_i]["arrivalDate"].InnerText.ToString());
                if (NodeUltItineraryOptionsDetail[_i]["departure"] != null)
                    me.UltItineraryOptionsDetail[_i].departure = NodeUltItineraryOptionsDetail[_i]["departure"].InnerText;
                if (NodeUltItineraryOptionsDetail[_i]["departureDate"] != null && !string.IsNullOrEmpty(NodeUltItineraryOptionsDetail[_i]["departureDate"].InnerText))
                    me.UltItineraryOptionsDetail[_i].departureDate = XmlConvert.ToDateTime(NodeUltItineraryOptionsDetail[_i]["departureDate"].InnerText.ToString());
                if (NodeUltItineraryOptionsDetail[_i]["idItineraryOption"] != null)
                    me.UltItineraryOptionsDetail[_i].idItineraryOption = XmlConvert.ToInt32(NodeUltItineraryOptionsDetail[_i]["idItineraryOption"].InnerText.ToString());
                if (NodeUltItineraryOptionsDetail[_i]["idItineraryOptionsDetail"] != null)
                    me.UltItineraryOptionsDetail[_i].idItineraryOptionsDetail = XmlConvert.ToInt32(NodeUltItineraryOptionsDetail[_i]["idItineraryOptionsDetail"].InnerText.ToString());
                if (NodeUltItineraryOptionsDetail[_i]["idMissionOrder"] != null)
                    me.UltItineraryOptionsDetail[_i].idMissionOrder = XmlConvert.ToInt32(NodeUltItineraryOptionsDetail[_i]["idMissionOrder"].InnerText.ToString());
                if (NodeUltItineraryOptionsDetail[_i]["lapseTime"] != null)
                    me.UltItineraryOptionsDetail[_i].lapseTime = XmlConvert.ToDouble(NodeUltItineraryOptionsDetail[_i]["lapseTime"].InnerText.ToString());
            }
            #endregion// ---- UltItineraryOptionsDetail ---- //

            #region// ---- UltPAClient ---- //
            int countUltPAClient = ObjXML.GetElementsByTagName("UltPAClient ").Count;
            XmlNode[] NodeUltPAClient = new XmlNode[countUltPAClient];
            me.UltPAClient = new eGastosEntity.Ultimus.UltPAClient[countUltPAClient];
            for (int _i = 0; _i < countUltPAClient; _i++)
            {
                NodeUltPAClient[_i] = ObjXML.GetElementsByTagName("UltPAClient ")[_i];
                me.UltPAClient[_i] = new eGastosEntity.Ultimus.UltPAClient();
                if (NodeUltPAClient[_i]["code"] != null)
                    me.UltPAClient[_i].code = NodeUltPAClient[_i]["code"].InnerText;
                if (NodeUltPAClient[_i]["idExpenseAccountDetail"] != null)
                    me.UltPAClient[_i].idExpenseAccountDetail = XmlConvert.ToInt32(NodeUltPAClient[_i]["idExpenseAccountDetail"].InnerText.ToString());
                if (NodeUltPAClient[_i]["name"] != null)
                    me.UltPAClient[_i].name = NodeUltPAClient[_i]["name"].InnerText;
            }
            #endregion// ---- UltPAClient ---- //

            #region// ---- UltSAPResponse ---- //
            int countUltSAPResponse = ObjXML.GetElementsByTagName("UltSAPResponse ").Count;
            XmlNode[] NodeUltSAPResponse = new XmlNode[countUltSAPResponse];
            me.UltSAPResponse = new UltSAPResponse[countUltSAPResponse];
            for (int _i = 0; _i < countUltSAPResponse; _i++)
            {
                NodeUltSAPResponse[_i] = ObjXML.GetElementsByTagName("UltSAPResponse ")[_i];
                me.UltSAPResponse[_i] = new UltSAPResponse();
                if (NodeUltSAPResponse[_i]["company"] != null)
                    me.UltSAPResponse[_i].company = XmlConvert.ToInt32(NodeUltSAPResponse[_i]["company"].InnerText.ToString());
                if (NodeUltSAPResponse[_i]["docNumber"] != null)
                    me.UltSAPResponse[_i].docNumber = XmlConvert.ToInt32(NodeUltSAPResponse[_i]["docNumber"].InnerText.ToString());
                if (NodeUltSAPResponse[_i]["idRequest"] != null)
                    me.UltSAPResponse[_i].idRequest = XmlConvert.ToInt32(NodeUltSAPResponse[_i]["idRequest"].InnerText.ToString());
                if (NodeUltSAPResponse[_i]["idResponse"] != null)
                    me.UltSAPResponse[_i].idResponse = XmlConvert.ToInt32(NodeUltSAPResponse[_i]["idResponse"].InnerText.ToString());
                if (NodeUltSAPResponse[_i]["type"] != null)
                    me.UltSAPResponse[_i].type = NodeUltSAPResponse[_i]["type"].InnerText;
                if (NodeUltSAPResponse[_i]["year"] != null)
                    me.UltSAPResponse[_i].year = XmlConvert.ToInt32(NodeUltSAPResponse[_i]["year"].InnerText.ToString());
            }
            #endregion// ---- UltSAPResponse ---- //

            for (int i = 0; i < NodeCount; i++)
            {
                #region UltApprove
                if (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name == "UltApprove")
                {
                    XmlNode NodeUltApprove = (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]);
                    me.UltApprove = new UltApprove();
                    me.UltApprove.approved = bool.Parse(NodeUltApprove["approved"].InnerText);
                    me.UltApprove.approverEmail = NodeUltApprove["approverEmail"].InnerText;
                    me.UltApprove.approverLogin = NodeUltApprove["approverLogin"].InnerText;
                    me.UltApprove.approverName = NodeUltApprove["approverName"].InnerText;
                }
                #endregion UltApprove

                #region UltExpenseAccount
                if (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name == "UltExpenseAccount")
                {
                    XmlNode NodeUltExpenseAccount = (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]);
                    me.UltExpenseAccount = new eGastosEntity.Ultimus.UltExpenseAccount();
                    if (NodeUltExpenseAccount["charged"] != null)
                        me.UltExpenseAccount.charged = XmlConvert.ToBoolean(NodeUltExpenseAccount["charged"].InnerText.ToString());
                    if (NodeUltExpenseAccount["creditCard"] != null)
                        me.UltExpenseAccount.creditCard = XmlConvert.ToBoolean(NodeUltExpenseAccount["creditCard"].InnerText.ToString());
                    if (NodeUltExpenseAccount["idExpenseAccount"] != null)
                        me.UltExpenseAccount.idExpenseAccount = XmlConvert.ToInt32(NodeUltExpenseAccount["idExpenseAccount"].InnerText.ToString());
                    if (NodeUltExpenseAccount["idRequest"] != null)
                        me.UltExpenseAccount.idRequest = XmlConvert.ToInt32(NodeUltExpenseAccount["idRequest"].InnerText.ToString());
                    if (NodeUltExpenseAccount["nationalManagerLogin"] != null)
                        me.UltExpenseAccount.nationalManagerLogin = NodeUltExpenseAccount["nationalManagerLogin"].InnerText;
                    if (NodeUltExpenseAccount["nationalManagerName"] != null)
                        me.UltExpenseAccount.nationalManagerName = NodeUltExpenseAccount["nationalManagerName"].InnerText;
                    if (NodeUltExpenseAccount["overdue"] != null)
                        me.UltExpenseAccount.overdue = XmlConvert.ToBoolean(NodeUltExpenseAccount["overdue"].InnerText.ToString());
                    if (NodeUltExpenseAccount["totalMeal"] != null)
                        me.UltExpenseAccount.totalMeal = XmlConvert.ToDouble(NodeUltExpenseAccount["totalMeal"].InnerText.ToString());
                    if (NodeUltExpenseAccount["totalMiniEvent"] != null)
                        me.UltExpenseAccount.totalMiniEvent = XmlConvert.ToDouble(NodeUltExpenseAccount["totalMiniEvent"].InnerText.ToString());
                    if (NodeUltExpenseAccount["totalNationalMeal"] != null)
                        me.UltExpenseAccount.totalNationalMeal = XmlConvert.ToDouble(NodeUltExpenseAccount["totalNationalMeal"].InnerText.ToString());
                }
                #endregion UltExpenseAccount

                #region UltExpenseFlowVariables
                if (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name == "UltExpenseFlowVariables")
                {
                    XmlNode NodeUltExpenseFlowVariables = (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]);
                    me.UltExpenseFlowVariables = new eGastosEntity.Ultimus.UltExpenseFlowVariables();
                    if (NodeUltExpenseFlowVariables["activeDirAreaGastos"] != null)
                        me.UltExpenseFlowVariables.activeDirAreaGastos = XmlConvert.ToBoolean(NodeUltExpenseFlowVariables["activeDirAreaGastos"].InnerText.ToString());
                    if (NodeUltExpenseFlowVariables["activeDirFinanzasGastos"] != null)
                        me.UltExpenseFlowVariables.activeDirFinanzasGastos = XmlConvert.ToBoolean(NodeUltExpenseFlowVariables["activeDirFinanzasGastos"].InnerText.ToString());
                    if (NodeUltExpenseFlowVariables["activeDirGralGastos"] != null)
                        me.UltExpenseFlowVariables.activeDirGralGastos = XmlConvert.ToBoolean(NodeUltExpenseFlowVariables["activeDirGralGastos"].InnerText.ToString());
                    if (NodeUltExpenseFlowVariables["activeManager"] != null)
                        me.UltExpenseFlowVariables.activeManager = XmlConvert.ToBoolean(NodeUltExpenseFlowVariables["activeManager"].InnerText.ToString());
                    if (NodeUltExpenseFlowVariables["jobFunctionAutorizador1"] != null)
                        me.UltExpenseFlowVariables.jobFunctionAutorizador1 = NodeUltExpenseFlowVariables["jobFunctionAutorizador1"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionAutorizador2"] != null)
                        me.UltExpenseFlowVariables.jobFunctionAutorizador2 = NodeUltExpenseFlowVariables["jobFunctionAutorizador2"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionAutorizador3"] != null)
                        me.UltExpenseFlowVariables.jobFunctionAutorizador3 = NodeUltExpenseFlowVariables["jobFunctionAutorizador3"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionAutorizador4"] != null)
                        me.UltExpenseFlowVariables.jobFunctionAutorizador4 = NodeUltExpenseFlowVariables["jobFunctionAutorizador4"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionController1"] != null)
                        me.UltExpenseFlowVariables.jobFunctionController1 = NodeUltExpenseFlowVariables["jobFunctionController1"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionController2"] != null)
                        me.UltExpenseFlowVariables.jobFunctionController2 = NodeUltExpenseFlowVariables["jobFunctionController2"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionControlling"] != null)
                        me.UltExpenseFlowVariables.jobFunctionControlling = NodeUltExpenseFlowVariables["jobFunctionControlling"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionDirAreaGastos"] != null)
                        me.UltExpenseFlowVariables.jobFunctionDirAreaGastos = NodeUltExpenseFlowVariables["jobFunctionDirAreaGastos"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionFinanzasGastos"] != null)
                        me.UltExpenseFlowVariables.jobFunctionFinanzasGastos = NodeUltExpenseFlowVariables["jobFunctionFinanzasGastos"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionNationalManager"] != null)
                        me.UltExpenseFlowVariables.jobFunctionNationalManager = NodeUltExpenseFlowVariables["jobFunctionNationalManager"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionObservador"] != null)
                        me.UltExpenseFlowVariables.jobFunctionObservador = NodeUltExpenseFlowVariables["jobFunctionObservador"].InnerText;
                    if (NodeUltExpenseFlowVariables["jobFunctionResponsible"] != null)
                        me.UltExpenseFlowVariables.jobFunctionResponsible = NodeUltExpenseFlowVariables["jobFunctionResponsible"].InnerText;
                    if (NodeUltExpenseFlowVariables["summaryText"] != null)
                        me.UltExpenseFlowVariables.summaryText = NodeUltExpenseFlowVariables["summaryText"].InnerText;
                }
                #endregion UltExpenseFlowVariables

                #region UltFlobotVariables
                if (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name == "UltFlobotVariables")
                {
                    XmlNode NodeUltFlobotVariables = (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]);
                    me.UltFlobotVariables = new UltFlobotVariables();
                    if (NodeUltFlobotVariables["messageError"] != null)
                        me.UltFlobotVariables.messageError = NodeUltFlobotVariables["messageError"].InnerText;
                    if (NodeUltFlobotVariables["messageErrorAgency"] != null)
                        me.UltFlobotVariables.messageErrorAgency = NodeUltFlobotVariables["messageErrorAgency"].InnerText;
                    if (NodeUltFlobotVariables["status"] != null)
                        me.UltFlobotVariables.status = XmlConvert.ToInt32(NodeUltFlobotVariables["status"].InnerText.ToString());
                    if (NodeUltFlobotVariables["statusAgencyFlobot"] != null)
                        me.UltFlobotVariables.statusAgencyFlobot = XmlConvert.ToInt32(NodeUltFlobotVariables["statusAgencyFlobot"].InnerText.ToString());
                }
                #endregion UltFlobotVariables

                #region UltItineraryRate
                if (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name == "UltItineraryRate")
                {
                    XmlNode NodeUltItineraryRate = (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]);
                    me.UltItineraryRate = new UltItineraryRate();
                    if (NodeUltItineraryRate["IdItineraryRate"] != null)
                        me.UltItineraryRate.IdItineraryRate = XmlConvert.ToInt32(NodeUltItineraryRate["IdItineraryRate"].InnerText.ToString());
                    if (NodeUltItineraryRate["idMissionOrder"] != null)
                        me.UltItineraryRate.idMissionOrder = XmlConvert.ToInt32(NodeUltItineraryRate["idMissionOrder"].InnerText.ToString());
                    if (NodeUltItineraryRate["IVA"] != null)
                        me.UltItineraryRate.IVA = XmlConvert.ToDouble(NodeUltItineraryRate["IVA"].InnerText.ToString());
                    if (NodeUltItineraryRate["lineStatus"] != null)
                        me.UltItineraryRate.lineStatus = XmlConvert.ToInt32(NodeUltItineraryRate["lineStatus"].InnerText.ToString());
                    if (NodeUltItineraryRate["lineStatusName"] != null)
                        me.UltItineraryRate.lineStatusName = NodeUltItineraryRate["lineStatusName"].InnerText;
                    if (NodeUltItineraryRate["otherTaxes"] != null)
                        me.UltItineraryRate.otherTaxes = XmlConvert.ToDouble(NodeUltItineraryRate["otherTaxes"].InnerText.ToString());
                    if (NodeUltItineraryRate["realRate"] != null)
                        me.UltItineraryRate.realRate = XmlConvert.ToDouble(NodeUltItineraryRate["realRate"].InnerText.ToString());
                    if (NodeUltItineraryRate["TUA"] != null)
                        me.UltItineraryRate.TUA = XmlConvert.ToDouble(NodeUltItineraryRate["TUA"].InnerText.ToString());
                }
                #endregion UltItineraryRate

                #region UltMissionOrder
                if (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name == "UltMissionOrder")
                {
                    XmlNode NodeUltMissionOrder = (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]);
                    me.UltMissionOrder = new eGastosEntity.Ultimus.UltMissionOrder();
                    if (NodeUltMissionOrder["advance"] != null)
                        me.UltMissionOrder.advance = XmlConvert.ToDouble(NodeUltMissionOrder["advance"].InnerText.ToString());
                    if (NodeUltMissionOrder["advanceApply"] != null)
                        me.UltMissionOrder.advanceApply = XmlConvert.ToBoolean(NodeUltMissionOrder["advanceApply"].InnerText.ToString());
                    if (NodeUltMissionOrder["comment"] != null)
                        me.UltMissionOrder.comment = NodeUltMissionOrder["comment"].InnerText;
                    if (NodeUltMissionOrder["countAgencyWait"] != null)
                        me.UltMissionOrder.countAgencyWait = XmlConvert.ToInt32(NodeUltMissionOrder["countAgencyWait"].InnerText.ToString());
                    if (NodeUltMissionOrder["hotel"] != null)
                        me.UltMissionOrder.hotel = XmlConvert.ToBoolean(NodeUltMissionOrder["hotel"].InnerText.ToString());
                    if (NodeUltMissionOrder["idAgencyLog"] != null)
                        me.UltMissionOrder.idAgencyLog = XmlConvert.ToInt32(NodeUltMissionOrder["idAgencyLog"].InnerText.ToString());
                    if (NodeUltMissionOrder["idAgencyResponse"] != null)
                        me.UltMissionOrder.idAgencyResponse = XmlConvert.ToInt32(NodeUltMissionOrder["idAgencyResponse"].InnerText.ToString());
                    if (NodeUltMissionOrder["idMissionOrder"] != null)
                        me.UltMissionOrder.idMissionOrder = XmlConvert.ToInt32(NodeUltMissionOrder["idMissionOrder"].InnerText.ToString());
                    if (NodeUltMissionOrder["idRequest"] != null)
                        me.UltMissionOrder.idRequest = XmlConvert.ToInt32(NodeUltMissionOrder["idRequest"].InnerText.ToString());
                    if (NodeUltMissionOrder["itinerary"] != null)
                        me.UltMissionOrder.itinerary = XmlConvert.ToBoolean(NodeUltMissionOrder["itinerary"].InnerText.ToString());
                    if (NodeUltMissionOrder["nationalCurrency"] != null)
                        me.UltMissionOrder.nationalCurrency = XmlConvert.ToDouble(NodeUltMissionOrder["nationalCurrency"].InnerText.ToString());
                    if (NodeUltMissionOrder["objective"] != null)
                        me.UltMissionOrder.objective = NodeUltMissionOrder["objective"].InnerText;
                    if (NodeUltMissionOrder["statusAgencyProcess"] != null)
                        me.UltMissionOrder.statusAgencyProcess = XmlConvert.ToInt32(NodeUltMissionOrder["statusAgencyProcess"].InnerText.ToString());
                    if (NodeUltMissionOrder["statusAgencySend"] != null)
                        me.UltMissionOrder.statusAgencySend = XmlConvert.ToInt32(NodeUltMissionOrder["statusAgencySend"].InnerText.ToString());
                    if (NodeUltMissionOrder["travelId"] != null)
                        me.UltMissionOrder.travelId = NodeUltMissionOrder["travelId"].InnerText;
                    if (NodeUltMissionOrder["travelName"] != null)
                        me.UltMissionOrder.travelName = NodeUltMissionOrder["travelName"].InnerText;
                }
                #endregion UltMissionOrder

                #region UltRequest
                if (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name == "UltRequest")
                {
                    XmlNode NodeUltRequest = (ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]);
                    me.UltRequest = new eGastosEntity.Ultimus.UltRequest();
                    if (NodeUltRequest["areaId"] != null)
                        me.UltRequest.areaId = XmlConvert.ToInt32(NodeUltRequest["areaId"].InnerText);
                    if (NodeUltRequest["areaText"] != null)
                        me.UltRequest.areaText = NodeUltRequest["areaText"].InnerText;
                    if (NodeUltRequest["arrival"] != null)
                        me.UltRequest.arrival = NodeUltRequest["arrival"].InnerText;
                    if (NodeUltRequest["CeCoCode"] != null)
                        me.UltRequest.CeCoCode = XmlConvert.ToInt32(NodeUltRequest["CeCoCode"].InnerText);
                    if (NodeUltRequest["CeCoMiniCode"] != null)
                        me.UltRequest.CeCoMiniCode = XmlConvert.ToInt32(NodeUltRequest["CeCoMiniCode"].InnerText);
                    if (NodeUltRequest["CeCoMiniName"] != null)
                        me.UltRequest.CeCoMiniName = NodeUltRequest["CeCoMiniName"].InnerText;
                    if (NodeUltRequest["CeCoName"] != null)
                        me.UltRequest.CeCoName = NodeUltRequest["CeCoName"].InnerText;
                    if (NodeUltRequest["companyCode"] != null)
                        me.UltRequest.companyCode = XmlConvert.ToInt32(NodeUltRequest["companyCode"].InnerText);
                    if (NodeUltRequest["companyName"] != null)
                        me.UltRequest.companyName = NodeUltRequest["companyName"].InnerText;
                    if (NodeUltRequest["currencyId"] != null)
                        me.UltRequest.currencyId = NodeUltRequest["currencyId"].InnerText;
                    if (NodeUltRequest["currencyName"] != null)
                        me.UltRequest.currencyName = NodeUltRequest["currencyName"].InnerText;
                    if (NodeUltRequest["departureDate"] != null)
                        me.UltRequest.departureDate = NodeUltRequest["departureDate"].InnerText;
                    if (NodeUltRequest["exchangeRate"] != null)
                        me.UltRequest.exchangeRate = XmlConvert.ToDouble(NodeUltRequest["exchangeRate"].InnerText);
                    if (NodeUltRequest["idRequest"] != null)
                        me.UltRequest.idRequest = XmlConvert.ToInt32(NodeUltRequest["idRequest"].InnerText);
                    if (NodeUltRequest["initiatorLogin"] != null)
                        me.UltRequest.initiatorLogin = NodeUltRequest["initiatorLogin"].InnerText;
                    if (NodeUltRequest["initiatorName"] != null)
                        me.UltRequest.initiatorName = NodeUltRequest["initiatorName"].InnerText;
                    if (NodeUltRequest["isMiniEvent"] != null)
                        me.UltRequest.isMiniEvent = XmlConvert.ToBoolean(NodeUltRequest["isMiniEvent"].InnerText);
                    if (NodeUltRequest["PAClientId"] != null)
                        me.UltRequest.PAClientId = NodeUltRequest["PAClientId"].InnerText;
                    if (NodeUltRequest["PAClientName"] != null)
                        me.UltRequest.PAClientName = NodeUltRequest["PAClientName"].InnerText;
                    if (NodeUltRequest["pasteur"] != null)
                        me.UltRequest.pasteur = XmlConvert.ToBoolean(NodeUltRequest["pasteur"].InnerText);
                    if (NodeUltRequest["PEPElementId"] != null)
                        me.UltRequest.PEPElementId = NodeUltRequest["PEPElementId"].InnerText;
                    if (NodeUltRequest["PEPElementName"] != null)
                        me.UltRequest.PEPElementName = NodeUltRequest["PEPElementName"].InnerText;
                    if (NodeUltRequest["requestDate"] != null)
                        me.UltRequest.requestDate = XmlConvert.ToDateTime(NodeUltRequest["requestDate"].InnerText);
                    if (NodeUltRequest["responsibleEmployeeNum"] != null)
                        me.UltRequest.responsibleEmployeeNum = NodeUltRequest["responsibleEmployeeNum"].InnerText;
                    if (NodeUltRequest["responsibleLogin"] != null)
                        me.UltRequest.responsibleLogin = NodeUltRequest["responsibleLogin"].InnerText;
                    if (NodeUltRequest["responsibleName"] != null)
                        me.UltRequest.responsibleName = NodeUltRequest["responsibleName"].InnerText;
                    if (NodeUltRequest["responsiblePayMethod"] != null)
                        me.UltRequest.responsiblePayMethod = NodeUltRequest["responsiblePayMethod"].InnerText;
                    if (NodeUltRequest["responsibleUserName"] != null)
                        me.UltRequest.responsibleUserName = NodeUltRequest["responsibleUserName"].InnerText;
                    if (NodeUltRequest["returnDate"] != null)
                        me.UltRequest.returnDate = NodeUltRequest["returnDate"].InnerText;
                    if (NodeUltRequest["salesForce"] != null)
                        me.UltRequest.salesForce = XmlConvert.ToBoolean(NodeUltRequest["salesForce"].InnerText);
                    if (NodeUltRequest["status"] != null)
                        me.UltRequest.status = XmlConvert.ToInt32(NodeUltRequest["status"].InnerText);
                    if (NodeUltRequest["statusName"] != null)
                        me.UltRequest.statusName = NodeUltRequest["statusName"].InnerText;
                    if (NodeUltRequest["type"] != null)
                        me.UltRequest.type = XmlConvert.ToInt32(NodeUltRequest["type"].InnerText);
                    if (NodeUltRequest["typeName"] != null)
                        me.UltRequest.typeName = NodeUltRequest["typeName"].InnerText;
                    if (NodeUltRequest["ultimusNumber"] != null && !string.IsNullOrEmpty(NodeUltRequest["ultimusNumber"].InnerText))
                        me.UltRequest.ultimusNumber = XmlConvert.ToInt32(NodeUltRequest["ultimusNumber"].InnerText);
                }
                #endregion
            }

            return "";
        }

        private int incidentGeneratePasteur(Entity.MasterEntity me, Entity.FilterData fd, out string msgError, out int nIncident, bool isApprove)
        {
            WSeGastosPasteur.eGastos_Pasteur ult_obj = new eGastos_Pasteur();
            WSeGastosPasteur.SchemaFile[] schemas;

            string strHoraAtual = ToXMLDateFormat(DateTime.Now);
            string summary = "O.M. para + " + me.UltRequester.requesterName;
            string strError = "";
            string strxml, strxml1;

            msgError = "";
            XmlDataDocument ObjXML = new System.Xml.XmlDataDocument();

            StringBuilder LarXML = new StringBuilder();
            XmlDocument oXmlDoc = new XmlDocument();

            //Variaveis ultimus
            int intIncident = 0;
            bool bolResultado;

            try
            {

                if (ult_obj.GetLaunchInformation(fd.UserLogin, out schemas, out strxml, out strError))
                {
                    ObjXML.LoadXml(strxml.Replace("\"", "'"));
                    string ProcessVersion = ObjXML.ChildNodes.Item(1).Attributes["xmlns"].Value;
                    int n = ProcessVersion.LastIndexOf("/");
                    string ProcessVersionNumber = ProcessVersion.Substring(0, n).ToString() + "/Types";

                    XmlNode XmlNodeCustom = (ObjXML.ChildNodes[1].ChildNodes[0]);

                    //// XML Original
                    LarXML.Append("<?xml version='1.0' encoding='utf-16'?> ");
                    LarXML.Append("<TaskData xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='" + ProcessVersion + "'>");

                    LarXML.Append("  <Global>");

                    #region <UltConfiguration>
                    LarXML.Append("    <UltConfiguration xmlns='" + ProcessVersionNumber + "'>");
                    LarXML.Append("      <serverName xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/</serverName>");
                    LarXML.Append("      <appDirectory xmlns='http://processSchema.eGastos/'>eGastos/Pages/</appDirectory>");
                    LarXML.Append("      <pageRequest xmlns='http://processSchema.eGastos/'>frmExpenseRequest.aspx</pageRequest>");
                    LarXML.Append("      <pageApproval xmlns='http://processSchema.eGastos/'>frmExpenseApproval.aspx</pageApproval>");
                    LarXML.Append("      <pageExpenseAccount xmlns='http://processSchema.eGastos/'>frmExpenseAccount.aspx</pageExpenseAccount>");
                    LarXML.Append("      <pageConfirmQuote xmlns='http://processSchema.eGastos/'>frmConfirmQuote.aspx</pageConfirmQuote>");
                    LarXML.Append("      <uRLRequest xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseRequest.aspx</uRLRequest>");
                    LarXML.Append("      <uRLApproval xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseApproval.aspx</uRLApproval>");
                    LarXML.Append("      <uRLExpenseAccount xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseAccount.aspx</uRLExpenseAccount>");
                    LarXML.Append("      <uRLConfirmQuote xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmConfirmQuote.aspx</uRLConfirmQuote>");
                    LarXML.Append("    </UltConfiguration>");
                    #endregion
                    #region <UltExpenseAccount>
                    if (me.UltExpenseAccount != null)
                    {
                        LarXML.Append("    <UltExpenseAccount xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idExpenseAccount xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.idExpenseAccount + "</idExpenseAccount>");
                        LarXML.Append("      <nationalManagerLogin xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.nationalManagerLogin + "</nationalManagerLogin>");
                        LarXML.Append("      <nationalManagerName xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.nationalManagerName + "</nationalManagerName>");
                        LarXML.Append("      <creditCard xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.creditCard.ToString().ToLower() + "</creditCard>");
                        LarXML.Append("      <totalMiniEvent xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.totalMiniEvent + "</totalMiniEvent>");
                        LarXML.Append("      <totalMeal xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.totalMeal + "</totalMeal>");
                        LarXML.Append("      <totalNationalMeal xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.totalNationalMeal + "</totalNationalMeal>");
                        LarXML.Append("      <overdue xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.overdue.ToString().ToLower() + "</overdue>");
                        LarXML.Append("      <charged xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.charged.ToString().ToLower() + "</charged>");
                        LarXML.Append("    </UltExpenseAccount>");
                    }
                    else
                    {
                        LarXML.Append("    <UltExpenseAccount xmlns='" + ProcessVersionNumber + "'> ");
                        LarXML.Append("          <idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount> ");
                        LarXML.Append("          <creditCard xmlns='http://processSchema.eGastos/'>false</creditCard> ");
                        LarXML.Append("          <totalMiniEvent xmlns='http://processSchema.eGastos/'>0</totalMiniEvent> ");
                        LarXML.Append("          <totalMeal xmlns='http://processSchema.eGastos/'>0</totalMeal> ");
                        LarXML.Append("          <totalNationalMeal xmlns='http://processSchema.eGastos/'>0</totalNationalMeal> ");
                        LarXML.Append("          <overdue xmlns='http://processSchema.eGastos/'>false</overdue> ");
                        LarXML.Append("          <charged xmlns='http://processSchema.eGastos/'>false</charged> ");
                        LarXML.Append("    </UltExpenseAccount> ");
                    }
                    #endregion
                    #region <UltMissionOrder>
                    if (me.UltMissionOrder != null)
                    {
                        // UltMissionOrder
                        LarXML.Append("    <UltMissionOrder xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idMissionOrder + "</idMissionOrder>");
                        LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idRequest + "</idRequest>");
                        LarXML.Append("      <idAgencyResponse xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idAgencyResponse + "</idAgencyResponse>");
                        LarXML.Append("      <statusAgencyProcess xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.statusAgencyProcess + "</statusAgencyProcess>");
                        LarXML.Append("      <statusAgencySend xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.statusAgencySend + "</statusAgencySend>");
                        LarXML.Append("      <countAgencyWait xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.countAgencyWait + "</countAgencyWait>");
                        LarXML.Append("      <idAgencyLog xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idAgencyLog + "</idAgencyLog>");
                        LarXML.Append("      <travelId xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.travelId + "</travelId>");
                        LarXML.Append("      <travelName xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.travelName + "</travelName>");
                        LarXML.Append("      <objective xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.objective + "</objective>");
                        LarXML.Append("      <advance xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.advance + "</advance>");
                        LarXML.Append("      <nationalCurrency xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.nationalCurrency + "</nationalCurrency>");
                        LarXML.Append("      <advanceApply xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.advanceApply.ToString().ToLower() + "</advanceApply>");
                        LarXML.Append("      <itinerary xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.itinerary.ToString().ToLower() + "</itinerary>");
                        LarXML.Append("      <hotel xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.hotel.ToString().ToLower() + "</hotel>");
                        if (!string.IsNullOrEmpty(me.UltMissionOrder.comment))
                        {
                            LarXML.Append("      <comment xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.comment + "</comment>");
                        }
                        LarXML.Append("    </UltMissionOrder>");
                    }
                    else
                    {
                        LarXML.Append("      <UltMissionOrder xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
                        LarXML.Append("      <idAgencyResponse xmlns='http://processSchema.eGastos/'>0</idAgencyResponse>");
                        LarXML.Append("      <statusAgencyProcess xmlns='http://processSchema.eGastos/'>0</statusAgencyProcess>");
                        LarXML.Append("      <statusAgencySend xmlns='http://processSchema.eGastos/'>0</statusAgencySend>");
                        LarXML.Append("      <countAgencyWait xmlns='http://processSchema.eGastos/'>0</countAgencyWait>");
                        LarXML.Append("      <idAgencyLog xmlns='http://processSchema.eGastos/'>0</idAgencyLog>");
                        LarXML.Append("      <advance xmlns='http://processSchema.eGastos/'>0</advance>");
                        LarXML.Append("      <nationalCurrency xmlns='http://processSchema.eGastos/'>0</nationalCurrency>");
                        LarXML.Append("      <advanceApply xmlns='http://processSchema.eGastos/'>false</advanceApply>");
                        LarXML.Append("      <itinerary xmlns='http://processSchema.eGastos/'>false</itinerary>");
                        LarXML.Append("      <hotel xmlns='http://processSchema.eGastos/'>false</hotel>");
                        LarXML.Append("      </UltMissionOrder>");
                    }
                    #endregion
                    #region <UltResponsible>
                    if (me.UltResponsible != null)
                    {
                        LarXML.Append("    <UltResponsible xmlns='" + ProcessVersionNumber + "' >");
                        LarXML.Append("      <responsibleName xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleName + "</responsibleName>");
                        LarXML.Append("      <responsibleLogin xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleLogin + "</responsibleLogin>");
                        LarXML.Append("      <responsibleEmail xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleEmail + "</responsibleEmail>");
                        LarXML.Append("    </UltResponsible>");
                    }
                    else
                    {
                        LarXML.Append("    <UltResponsible xmlns='" + ProcessVersionNumber + "' />");
                    }
                    #endregion
                    #region  <UltRequester>
                    if (me.UltRequester != null)
                    {
                        LarXML.Append("    <UltRequester xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <requesterName xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterName + "</requesterName>");
                        LarXML.Append("      <requesterLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterLogin + "</requesterLogin>");
                        LarXML.Append("      <requesterEmail xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterEmail + "</requesterEmail>");
                        LarXML.Append("      <requesterDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(me.UltRequester.requesterDate) + "</requesterDate>");
                        LarXML.Append("    </UltRequester>");
                    }
                    else
                    {
                        LarXML.Append("    <UltRequester xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <requesterDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requesterDate>");
                        LarXML.Append("    </UltRequester>");
                    }
                    #endregion
                    #region <UltItinerary>
                    if (me.UltItinerary != null)
                    {

                        foreach (eGastosEntity.Ultimus.UltItinerary obj in me.UltItinerary)
                        {
                            LarXML.Append("    <UltItinerary xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idItinerary xmlns='http://processSchema.eGastos/'>" + obj.idItinerary + "</idItinerary>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <idConsecutive xmlns='http://processSchema.eGastos/'>" + obj.idConsecutive + "</idConsecutive>");
                            LarXML.Append("      <idLedgerAccount xmlns='http://processSchema.eGastos/'>" + obj.idLedgerAccount + "</idLedgerAccount>");
                            LarXML.Append("      <nameLedgerAccount xmlns='http://processSchema.eGastos/'>" + obj.nameLedgerAccount + "</nameLedgerAccount>");
                            LarXML.Append("      <departureHour xmlns='http://processSchema.eGastos/'>" + obj.departureHour + "</departureHour>");
                            LarXML.Append("      <returnHour xmlns='http://processSchema.eGastos/'>" + obj.returnHour + "</returnHour>");
                            LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
                            LarXML.Append("      <travelType xmlns='http://processSchema.eGastos/'>" + obj.travelType + "</travelType>");
                            LarXML.Append("      <nameTravelType xmlns='http://processSchema.eGastos/'>" + obj.nameTravelType + "</nameTravelType>");
                            LarXML.Append("      <departureCountry xmlns='http://processSchema.eGastos/'>" + obj.departureCountry + "</departureCountry>");
                            LarXML.Append("      <departureCity xmlns='http://processSchema.eGastos/'>" + obj.departureCity + "</departureCity>");
                            LarXML.Append("      <arrivalCountry xmlns='http://processSchema.eGastos/'>" + obj.arrivalCountry + "</arrivalCountry>");
                            LarXML.Append("      <arrivalCity xmlns='http://processSchema.eGastos/'>" + obj.arrivalCity + "</arrivalCity>");
                            LarXML.Append("      <departureDate  xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.departureDate) + "</departureDate>");
                            LarXML.Append("      <arrivalDate  xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.arrivalDate) + "</arrivalDate>");
                            LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
                            LarXML.Append("    </UltItinerary>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltItinerary xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idItinerary xmlns='http://processSchema.eGastos/'>0</idItinerary>");
                        LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("    <idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive>");
                        LarXML.Append("    <idLedgerAccount xmlns='http://processSchema.eGastos/'>0</idLedgerAccount>");
                        LarXML.Append("    <travelType xmlns='http://processSchema.eGastos/'>0</travelType>");
                        LarXML.Append("    <departureDate xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    <arrivalDate xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>false</status>");
                        LarXML.Append("    </UltItinerary>");
                    }
                    #endregion
                    LarXML.Append("        <Cancel xmlns='" + ProcessVersionNumber + "'>false</Cancel>");
                    LarXML.Append("        <bApprover1 xmlns='" + ProcessVersionNumber + "'>false</bApprover1>");
                    LarXML.Append("        <bApprover2 xmlns='" + ProcessVersionNumber + "'>false</bApprover2>");
                    LarXML.Append("        <bApprover3 xmlns='" + ProcessVersionNumber + "'>false</bApprover3>");
                    LarXML.Append("        <bApprover4 xmlns='" + ProcessVersionNumber + "'>false</bApprover4>");
                    LarXML.Append("        <bController1 xmlns='" + ProcessVersionNumber + "'>false</bController1>");
                    LarXML.Append("        <bController2 xmlns='" + ProcessVersionNumber + "'>false</bController2>");
                    #region <UltItineraryOptionsDetail>
                    if (me.UltItineraryOptionsDetail != null)
                    {
                        foreach (UltItineraryOptionsDetail obj in me.UltItineraryOptionsDetail)
                        {
                            LarXML.Append("    <UltItineraryOptionsDetail xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOptionsDetail + "</idItineraryOptionsDetail>");
                            LarXML.Append("      <idItineraryOption xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOption + "</idItineraryOption>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <airlineFlight xmlns='http://processSchema.eGastos/'>" + obj.airlineFlight + "</airlineFlight>");
                            LarXML.Append("      <departure xmlns='http://processSchema.eGastos/'>" + obj.departure + "</departure>");
                            LarXML.Append("      <arrival xmlns='http://processSchema.eGastos/'>" + obj.arrival + "</arrival>");
                            LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.departureDate) + "</departureDate>");
                            LarXML.Append("      <arrivalDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.arrivalDate) + "</arrivalDate>");
                            LarXML.Append("      <lapseTime xmlns='http://processSchema.eGastos/'>" + obj.lapseTime + "</lapseTime>");
                            LarXML.Append("    </UltItineraryOptionsDetail>");
                        }
                    }
                    else
                    {
                        LarXML.Append("        <UltItineraryOptionsDetail xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("        <idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>0</idItineraryOptionsDetail>");
                        LarXML.Append("        <idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption>");
                        LarXML.Append("        <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("        <departureDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</departureDate>");
                        LarXML.Append("        <arrivalDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</arrivalDate>");
                        LarXML.Append("        <lapseTime xmlns='http://processSchema.eGastos/'>0</lapseTime>");
                        LarXML.Append("        </UltItineraryOptionsDetail>");
                    }
                    #endregion
                    #region <UltPAClient>
                    if (me.UltPAClient != null)
                    {
                        foreach (eGastosEntity.Ultimus.UltPAClient obj in me.UltPAClient)
                        {
                            LarXML.Append("    <UltPAClient xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccountDetail + "</idExpenseAccountDetail>");
                            LarXML.Append("      <code xmlns='http://processSchema.eGastos/'>" + obj.code + "</code>");
                            LarXML.Append("      <name xmlns='http://processSchema.eGastos/'>" + obj.name + "</name>");
                            LarXML.Append("    </UltPAClient>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltPAClient xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail>");
                        LarXML.Append("    </UltPAClient>");
                    }
                    #endregion
                    #region  <UltItineraryOptions>
                    if (me.UltItineraryOptions != null)
                    {
                        foreach (UltItineraryOptions obj in me.UltItineraryOptions)
                        {
                            LarXML.Append("    <UltItineraryOptions xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idItineraryOption xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOption + "</idItineraryOption>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <idRate xmlns='http://processSchema.eGastos/'>" + obj.idRate + "</idRate>");
                            LarXML.Append("      <quoteRate xmlns='http://processSchema.eGastos/'>" + obj.quoteRate + "</quoteRate>");
                            LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
                            LarXML.Append("      <confirmed xmlns='http://processSchema.eGastos/'>" + obj.confirmed.ToString().ToLower() + "</confirmed>");
                            LarXML.Append("      <lastDayPurchase xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.lastDayPurchase) + "</lastDayPurchase>");
                            LarXML.Append("    </UltItineraryOptions>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltItineraryOptions xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption>");
                        LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("    <idRate xmlns='http://processSchema.eGastos/'>0</idRate>");
                        LarXML.Append("    <quoteRate xmlns='http://processSchema.eGastos/'>0</quoteRate>");
                        LarXML.Append("    <confirmed xmlns='http://processSchema.eGastos/'>false</confirmed>");
                        LarXML.Append("    <lastDayPurchase xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</lastDayPurchase>");
                        LarXML.Append("    </UltItineraryOptions>");
                    }
                    #endregion
                    #region <UltApprovalHistory>
                    if (me.UltApprovalHistory != null)
                    {
                        foreach (UltApprovalHistory obj in me.UltApprovalHistory)
                        {
                            LarXML.Append("    <UltApprovalHistory xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <stepName xmlns='http://processSchema.eGastos/'>" + obj.stepName + "</stepName>");
                            LarXML.Append("      <approverName xmlns='http://processSchema.eGastos/'>" + obj.approverName + "</approverName>");
                            LarXML.Append("      <approverLogin xmlns='http://processSchema.eGastos/'>" + obj.approverLogin + "</approverLogin>");
                            LarXML.Append("      <userEmail xmlns='http://processSchema.eGastos/'>" + obj.userEmail + "</userEmail>");
                            LarXML.Append("      <approveDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.approveDate) + "</approveDate>");
                            LarXML.Append("      <comments xmlns='http://processSchema.eGastos/'>" + obj.comments + "</comments>");
                            LarXML.Append("      <approveStatus xmlns='http://processSchema.eGastos/'>" + obj.approveStatus + "</approveStatus>");
                            LarXML.Append("    </UltApprovalHistory>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltApprovalHistory xmlns='" + ProcessVersionNumber + "'> ");
                        LarXML.Append("      <approveDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</approveDate> ");
                        LarXML.Append("    </UltApprovalHistory>");
                    }
                    #endregion
                    #region <UltHotel>
                    if (me.UltHotel != null)
                    {
                        foreach (eGastosEntity.Ultimus.UltHotel obj in me.UltHotel)
                        {
                            LarXML.Append("    <UltHotel xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idHotel xmlns='http://processSchema.eGastos/'>" + obj.idHotel + "</idHotel>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <idRated xmlns='http://processSchema.eGastos/'>" + obj.idRated + "</idRated>");
                            LarXML.Append("      <idConsecutive xmlns='http://processSchema.eGastos/'>" + obj.idConsecutive + "</idConsecutive>");
                            LarXML.Append("      <idLegerAccount xmlns='http://processSchema.eGastos/'>" + obj.idLegerAccount + "</idLegerAccount>");
                            LarXML.Append("      <nameLegerAccount xmlns='http://processSchema.eGastos/'>" + obj.nameLegerAccount + "</nameLegerAccount>");
                            LarXML.Append("      <country xmlns='http://processSchema.eGastos/'>" + obj.country + "</country>");
                            LarXML.Append("      <city xmlns='http://processSchema.eGastos/'>" + obj.city + "</city>");
                            LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
                            LarXML.Append("      <checkInDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.checkInDate) + "</checkInDate>");
                            LarXML.Append("      <checkoutDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.checkoutDate) + "</checkoutDate>");
                            LarXML.Append("      <hotelName xmlns='http://processSchema.eGastos/'>" + obj.hotelName + "</hotelName>");
                            LarXML.Append("      <reservation xmlns='http://processSchema.eGastos/'>" + obj.reservation + "</reservation>");
                            LarXML.Append("      <telephone xmlns='http://processSchema.eGastos/'>" + obj.telephone + "</telephone>");
                            LarXML.Append("      <address xmlns='http://processSchema.eGastos/'>" + obj.address + "</address>");
                            LarXML.Append("      <quotedRate xmlns='http://processSchema.eGastos/'>" + obj.quotedRate + "</quotedRate>");
                            LarXML.Append("      <realRate xmlns='http://processSchema.eGastos/'>" + obj.realRate + "</realRate>");
                            LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + obj.IVA + "</IVA>");
                            LarXML.Append("      <hotelTax xmlns='http://processSchema.eGastos/'>" + obj.hotelTax + "</hotelTax>");
                            LarXML.Append("      <otherTaxes xmlns='http://processSchema.eGastos/'>" + obj.otherTaxes + "</otherTaxes>");
                            LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
                            LarXML.Append("      <lineStatus xmlns='http://processSchema.eGastos/'>" + obj.lineStatus + "</lineStatus>");
                            LarXML.Append("      <lineStatusName xmlns='http://processSchema.eGastos/'>" + obj.lineStatusName + "</lineStatusName>");
                            LarXML.Append("    </UltHotel>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltHotel xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idHotel xmlns='http://processSchema.eGastos/'>0</idHotel>");
                        LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("    <idRated xmlns='http://processSchema.eGastos/'>0</idRated>");
                        LarXML.Append("    <idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive>");
                        LarXML.Append("    <idLegerAccount xmlns='http://processSchema.eGastos/'>0</idLegerAccount>");
                        LarXML.Append("    <checkInDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkInDate>");
                        LarXML.Append("    <checkoutDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkoutDate>");
                        LarXML.Append("    <quotedRate xmlns='http://processSchema.eGastos/'>0</quotedRate>");
                        LarXML.Append("    <realRate xmlns='http://processSchema.eGastos/'>0</realRate>");
                        LarXML.Append("    <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
                        LarXML.Append("    <hotelTax xmlns='http://processSchema.eGastos/'>0</hotelTax>");
                        LarXML.Append("    <otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes>");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>false</status>");
                        LarXML.Append("    <lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus>");
                        LarXML.Append("    </UltHotel>");
                    }
                    #endregion
                    #region <UltFlobotVariables>
                    if (me.UltFlobotVariables != null)
                    {
                        LarXML.Append("    <UltFlobotVariables xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <statusAgencyFlobot xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.statusAgencyFlobot + "</statusAgencyFlobot>");
                        LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.status + "</status>");
                        LarXML.Append("      <messageError xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.messageError + "</messageError>");
                        LarXML.Append("      <messageErrorAgency xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.messageErrorAgency + "</messageErrorAgency>");
                        LarXML.Append("    </UltFlobotVariables>");
                    }
                    else
                    {
                        LarXML.Append("    <UltFlobotVariables xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <statusAgencyFlobot xmlns='http://processSchema.eGastos/'>0</statusAgencyFlobot>");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>0</status>");
                        LarXML.Append("    </UltFlobotVariables>");
                    }
                    #endregion
                    #region <UltSAPResponse>
                    if (me.UltSAPResponse != null)
                    {
                        foreach (UltSAPResponse obj in me.UltSAPResponse)
                        {
                            LarXML.Append("    <UltSAPResponse xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idResponse xmlns='http://processSchema.eGastos/'>" + obj.idResponse + "</idResponse>");
                            LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + obj.idRequest + "</idRequest>");
                            LarXML.Append("      <docNumber xmlns='http://processSchema.eGastos/'>" + obj.docNumber + "</docNumber>");
                            LarXML.Append("      <company xmlns='http://processSchema.eGastos/'>" + obj.company + "</company>");
                            LarXML.Append("      <year xmlns='http://processSchema.eGastos/'>" + obj.year + "</year>");
                            LarXML.Append("      <type xmlns='http://processSchema.eGastos/'>" + obj.type + "</type>");
                            LarXML.Append("    </UltSAPResponse>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltSAPResponse xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idResponse xmlns='http://processSchema.eGastos/'>0</idResponse>");
                        LarXML.Append("    <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
                        LarXML.Append("    <docNumber xmlns='http://processSchema.eGastos/'>0</docNumber>");
                        LarXML.Append("    <company xmlns='http://processSchema.eGastos/'>0</company>");
                        LarXML.Append("    <year xmlns='http://processSchema.eGastos/'>0</year>");
                        LarXML.Append("    </UltSAPResponse>");
                    }
                    #endregion
                    #region <UltRequest>
                    //Request
                    if (me.UltRequest != null)
                    {
                        LarXML.Append("    <UltRequest xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + me.UltRequest.idRequest + "</idRequest>");
                        LarXML.Append("      <requestDate xmlns='http://processSchema.eGastos/'>" + strHoraAtual + "</requestDate>");
                        if (!string.IsNullOrEmpty(me.UltRequest.companyName))
                        {
                            LarXML.Append("      <companyName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.companyName + "</companyName>");
                        }
                        LarXML.Append("      <companyCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.companyCode + "</companyCode>");
                        LarXML.Append("      <CeCoCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoCode + "</CeCoCode>");
                        LarXML.Append("      <CeCoName  xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoName + "</CeCoName>");
                        LarXML.Append("      <CeCoMiniCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoMiniCode + "</CeCoMiniCode>");
                        LarXML.Append("      <CeCoMiniName  xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoMiniName + "</CeCoMiniName>");

                        LarXML.Append("      <isMiniEvent xmlns='http://processSchema.eGastos/'>" + me.UltRequest.isMiniEvent.ToString().ToLower() + "</isMiniEvent>");
                        if (!string.IsNullOrEmpty(me.UltRequest.arrival))
                        {
                            LarXML.Append("      <arrival xmlns='http://processSchema.eGastos/'>" + me.UltRequest.arrival + "</arrival>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.departureDate))
                        {
                            LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.departureDate + "</departureDate>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.returnDate))
                        {
                            LarXML.Append("      <returnDate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.returnDate + "</returnDate>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.PEPElementId))
                        {
                            LarXML.Append("      <PEPElementId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PEPElementId + "</PEPElementId>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.PEPElementName))
                        {
                            LarXML.Append("      <PEPElementName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PEPElementName + "</PEPElementName>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.currencyId))
                        {
                            LarXML.Append("      <currencyId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.currencyId + "</currencyId>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.currencyName))
                        {
                            LarXML.Append("      <currencyName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.currencyName + "</currencyName>");
                        }
                        LarXML.Append("      <exchangeRate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.exchangeRate + "</exchangeRate>");
                        if (!string.IsNullOrEmpty(me.UltRequest.initiatorLogin))
                        {
                            LarXML.Append("      <initiatorLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequest.initiatorLogin + "</initiatorLogin>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.initiatorName))
                        {
                            LarXML.Append("      <initiatorName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.initiatorName + "</initiatorName>");
                        }
                        LarXML.Append("      <PAClientId  xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PAClientId + "</PAClientId>");
                        LarXML.Append("      <PAClientName  xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PAClientName + "</PAClientName>");
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleLogin))
                        {
                            LarXML.Append("      <responsibleLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleLogin + "</responsibleLogin>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleName))
                        {
                            LarXML.Append("      <responsibleName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleName + "</responsibleName>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleEmployeeNum))
                        {
                            LarXML.Append("      <responsibleEmployeeNum xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleEmployeeNum + "</responsibleEmployeeNum>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleUserName))
                        {
                            LarXML.Append("      <responsibleUserName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleUserName + "</responsibleUserName>");
                        }
                        LarXML.Append("      <pasteur xmlns='http://processSchema.eGastos/'>" + me.UltRequest.pasteur.ToString().ToLower() + "</pasteur>");
                        LarXML.Append("      <areaId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.areaId + "</areaId>");
                        if (!string.IsNullOrEmpty(me.UltRequest.areaText))
                        {
                            LarXML.Append("      <areaText xmlns='http://processSchema.eGastos/'>" + me.UltRequest.areaText + "</areaText>");
                        }
                        LarXML.Append("      <ultimusNumber  xmlns='http://processSchema.eGastos/'>" + me.UltRequest.ultimusNumber + "</ultimusNumber>");
                        LarXML.Append("      <type xmlns='http://processSchema.eGastos/'>" + me.UltRequest.type + "</type>");
                        LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + me.UltRequest.status + "</status>");
                        LarXML.Append("      <salesForce  xmlns='http://processSchema.eGastos/'>" + me.UltRequest.salesForce.ToString().ToLower() + "</salesForce>");
                        LarXML.Append("    </UltRequest>");
                    }
                    else
                    {
                        LarXML.Append("    <UltRequest xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
                        LarXML.Append("    <requestDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requestDate>");
                        LarXML.Append("    <companyCode xmlns='http://processSchema.eGastos/'>0</companyCode>");
                        LarXML.Append("    <CeCoCode xmlns='http://processSchema.eGastos/'>0</CeCoCode>");
                        LarXML.Append("    <CeCoMiniCode xmlns='http://processSchema.eGastos/'>0</CeCoMiniCode>");
                        LarXML.Append("    <isMiniEvent xmlns='http://processSchema.eGastos/'>false</isMiniEvent>");
                        LarXML.Append("    <exchangeRate xmlns='http://processSchema.eGastos/'>0</exchangeRate>");
                        LarXML.Append("    <pasteur xmlns='http://processSchema.eGastos/'>false</pasteur>");
                        LarXML.Append("    <areaId xmlns='http://processSchema.eGastos/'>0</areaId>");
                        LarXML.Append("    <ultimusNumber xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    <type xmlns='http://processSchema.eGastos/'>0</type>");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>0</status>");
                        LarXML.Append("    <salesForce xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    </UltRequest>");
                    }
                    #endregion
                    #region <UltExpenseAccountDetail>
                    if (me.UltExpenseAccountDetail != null)
                    {
                        foreach (eGastosEntity.Ultimus.UltExpenseAccountDetail obj in me.UltExpenseAccountDetail)
                        {
                            LarXML.Append("    <UltExpenseAccountDetail xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccountDetail + "</idExpenseAccountDetail>");
                            LarXML.Append("      <idExpenseAccount xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccount + "</idExpenseAccount>");
                            LarXML.Append("      <expenseDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(obj.expenseDate) + "</expenseDate>");
                            LarXML.Append("      <idAccount xmlns='http://processSchema.eGastos/'>" + obj.idAccount + "</idAccount>");
                            LarXML.Append("      <accountName xmlns='http://processSchema.eGastos/'>" + obj.accountName + "</accountName>");
                            LarXML.Append("      <amount xmlns='http://processSchema.eGastos/'>" + obj.amount + "</amount>");
                            LarXML.Append("      <invoiceNumber xmlns='http://processSchema.eGastos/'>" + obj.invoiceNumber + "</invoiceNumber>");
                            LarXML.Append("      <place xmlns='http://processSchema.eGastos/'>" + obj.place + "</place>");
                            LarXML.Append("      <numberOfDiners xmlns='http://processSchema.eGastos/'>" + obj.numberOfDiners + "</numberOfDiners>");
                            LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + obj.IVA + "</IVA>");
                            LarXML.Append("      <healthProfessional xmlns='http://processSchema.eGastos/'>" + obj.healthProfessional.ToString().ToLower() + "</healthProfessional>");
                            LarXML.Append("      <discount xmlns='http://processSchema.eGastos/'>" + obj.discount + "</discount>");
                            LarXML.Append("      <hasPAClient xmlns='http://processSchema.eGastos/'>" + obj.hasPAClient.ToString().ToLower() + "</hasPAClient>");
                            LarXML.Append("      <IVATypeId xmlns='http://processSchema.eGastos/'>" + obj.IVATypeId + "</IVATypeId>");
                            LarXML.Append("      <IVATypeName xmlns='http://processSchema.eGastos/'>" + obj.IVATypeName + "</IVATypeName>");
                            LarXML.Append("      <total xmlns='http://processSchema.eGastos/'>" + obj.total + "</total>");
                            LarXML.Append("      <observationId xmlns='http://processSchema.eGastos/'>" + obj.observationId + "</observationId>");
                            LarXML.Append("      <observationName xmlns='http://processSchema.eGastos/'>" + obj.observationName + "</observationName>");
                            LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
                            LarXML.Append("    </UltExpenseAccountDetail>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltExpenseAccountDetail xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("          <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail>");
                        LarXML.Append("          <idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount>");
                        LarXML.Append("          <expenseDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</expenseDate>");
                        LarXML.Append("          <idAccount xmlns='http://processSchema.eGastos/'>0</idAccount>");
                        LarXML.Append("          <amount xmlns='http://processSchema.eGastos/'>0</amount>");
                        LarXML.Append("          <numberOfDiners xmlns='http://processSchema.eGastos/'>0</numberOfDiners>");
                        LarXML.Append("          <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
                        LarXML.Append("          <healthProfessional xmlns='http://processSchema.eGastos/'>false</healthProfessional>");
                        LarXML.Append("          <discount xmlns='http://processSchema.eGastos/'>0</discount>");
                        LarXML.Append("          <hasPAClient xmlns='http://processSchema.eGastos/'>false</hasPAClient>");
                        LarXML.Append("          <total xmlns='http://processSchema.eGastos/'>0</total>");
                        LarXML.Append("          <observationId xmlns='http://processSchema.eGastos/'>0</observationId>");
                        LarXML.Append("          <status xmlns='http://processSchema.eGastos/'>false</status>");
                        LarXML.Append("    </UltExpenseAccountDetail>");
                    }
                    #endregion

                    LarXML.Append("    <sObserver xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <bObserver xmlns='" + ProcessVersionNumber + "'>false</bObserver>");
                    LarXML.Append("    <OutOfCountry xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <AdminTelephone xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <MailAgencia xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    #region <UltApprove>
                    if (me.UltApprove != null)
                    {
                        LarXML.Append("    <UltApprove xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <approved xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approved.ToString().ToLower() + "</approved>");
                        LarXML.Append("      <approverName xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverName + "</approverName>");
                        LarXML.Append("      <approverLogin xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverLogin + "</approverLogin>");
                        LarXML.Append("      <approverEmail xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverEmail + "</approverEmail>");
                        LarXML.Append("    </UltApprove>");
                    }
                    else
                    {
                        LarXML.Append("    <UltApprove xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <approved xmlns='http://processSchema.eGastos/'>false</approved>");
                        LarXML.Append("    </UltApprove>");
                    }
                    #endregion
                    LarXML.Append("    <CountJobFunctionObservador xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <CountJobFunctionController1 xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <CountJobFunctionController2 xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <CountJobFunctionAutorizador1 xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <CountJobFunctionAutorizador2 xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <CountJobFunctionAutorizador3 xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("    <CountJobFunctionAutorizador4 xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");

                    LarXML.Append("        <ExpenseAccountTotal xmlns='" + ProcessVersionNumber + "'>");
                    LarXML.Append("        <WithoutIVA xsi:nil='true' />");
                    LarXML.Append("        <IVA xsi:nil='true' />");
                    LarXML.Append("        <Amount xsi:nil='true' />");
                    LarXML.Append("        </ExpenseAccountTotal>");

                    #region <UltItineraryRate>
                    if (me.UltItineraryRate != null)
                    {
                        LarXML.Append("    <UltItineraryRate xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <IdItineraryRate xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.IdItineraryRate + "</IdItineraryRate>");
                        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.idMissionOrder + "</idMissionOrder>");
                        LarXML.Append("      <realRate xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.realRate + "</realRate>");
                        LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.IVA + "</IVA>");
                        LarXML.Append("      <TUA xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.TUA + "</TUA>");
                        LarXML.Append("      <otherTaxes xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.otherTaxes + "</otherTaxes>");
                        LarXML.Append("      <lineStatus xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.lineStatus + "</lineStatus>");
                        LarXML.Append("      <lineStatusName xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.lineStatusName + "</lineStatusName>");
                        LarXML.Append("    </UltItineraryRate>");
                    }
                    else
                    {
                        LarXML.Append("        <UltItineraryRate xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("        <IdItineraryRate xmlns='http://processSchema.eGastos/'>0</IdItineraryRate>");
                        LarXML.Append("        <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("        <realRate xmlns='http://processSchema.eGastos/'>0</realRate>");
                        LarXML.Append("        <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
                        LarXML.Append("        <TUA xmlns='http://processSchema.eGastos/'>0</TUA>");
                        LarXML.Append("        <otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes>");
                        LarXML.Append("        <lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus>");
                        LarXML.Append("        </UltItineraryRate>");

                    }
                    #endregion
                    #region  <UltExpenseFlowVariables>
                    if (me.UltExpenseFlowVariables != null)
                    {
                        LarXML.Append("    <UltExpenseFlowVariables xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <summaryText xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.summaryText + "</summaryText>");
                        LarXML.Append("      <jobFunctionResponsible xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionResponsible + "</jobFunctionResponsible>");
                        LarXML.Append("      <activeDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirAreaGastos.ToString().ToLower() + "</activeDirAreaGastos>");
                        LarXML.Append("      <activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirFinanzasGastos.ToString().ToLower() + "</activeDirFinanzasGastos>");
                        LarXML.Append("      <activeManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeManager.ToString().ToLower() + "</activeManager>");
                        LarXML.Append("      <jobFunctionControlling xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionControlling + "</jobFunctionControlling>");
                        LarXML.Append("      <jobFunctionNationalManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionNationalManager + "</jobFunctionNationalManager>");
                        LarXML.Append("      <jobFunctionAutorizador1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador1 + "</jobFunctionAutorizador1>");
                        LarXML.Append("      <jobFunctionAutorizador2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador2 + "</jobFunctionAutorizador2>");
                        LarXML.Append("      <jobFunctionAutorizador3 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador3 + "</jobFunctionAutorizador3>");
                        LarXML.Append("      <jobFunctionAutorizador4 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador4 + "</jobFunctionAutorizador4>");
                        LarXML.Append("      <jobFunctionController1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController1 + "</jobFunctionController1>");
                        LarXML.Append("      <jobFunctionController2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController2 + "</jobFunctionController2>");
                        LarXML.Append("      <jobFunctionObservador xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionObservador + "</jobFunctionObservador>");
                        LarXML.Append("      <jobFunctionDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionDirAreaGastos + "</jobFunctionDirAreaGastos>");
                        LarXML.Append("      <jobFunctionFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionFinanzasGastos + "</jobFunctionFinanzasGastos>");
                        LarXML.Append("      <activeDirGralGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirGralGastos.ToString().ToLower() + "</activeDirGralGastos>");
                        LarXML.Append("    </UltExpenseFlowVariables>");
                    }
                    else
                    {
                        LarXML.Append("    <UltExpenseFlowVariables xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <activeDirAreaGastos xmlns='http://processSchema.eGastos/'>false</activeDirAreaGastos>");
                        LarXML.Append("    <activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>false</activeDirFinanzasGastos>");
                        LarXML.Append("    <activeManager xmlns='http://processSchema.eGastos/'>false</activeManager>");
                        LarXML.Append("    <activeDirGralGastos xmlns='http://processSchema.eGastos/'>false</activeDirGralGastos>");
                        LarXML.Append("    </UltExpenseFlowVariables>");
                    }
                    #endregion
                    LarXML.Append("      </Global>");
                    LarXML.Append("      <SYS_PROCESSATTACHMENTS />");
                    LarXML.Append("    </TaskData>");
                    //-------------
                    strxml1 = LarXML.ToString();

                    strError = "";
                    bolResultado = ult_obj.LaunchIncident(fd.UserLogin, summary, "Incidente criado por: ", false, 9, strxml1, true, out intIncident, out strError);

                }

                nIncident = intIncident;
                if (strError != "")
                {
                    msgError = strError;
                    return intIncident;
                }

            }
            catch (Exception ex)
            {
                msgError = strError = ex.Message;
                nIncident = 0;
                return intIncident;
            }
            return intIncident;
        }

        private int incidentGeneratePharma(Entity.MasterEntity me, Entity.FilterData fd, out string msgError, out int nIncident, bool isApprove)
        {
            WSeGastosPharma.eGastos_Pharma ult_obj = new WSeGastosPharma.eGastos_Pharma();
            WSeGastosPharma.SchemaFile[] schemas;

            // REQUEST  DATA (rd)           
            string strHoraAtual = ToXMLDateFormat(DateTime.Now);

            string summary = "Sumario da Solicitacao do eGastos Pharma ";
            string strError = "";
            string strxml, strxml1;

            msgError = "";
            XmlDataDocument ObjXML = new System.Xml.XmlDataDocument();

            StringBuilder LarXML = new StringBuilder();
            XmlDocument oXmlDoc = new XmlDocument();

            int intIncident = 0;
            bool bolResultado;

            try
            {
                if (ult_obj.GetLaunchInformation(fd.UserLogin, out schemas, out strxml, out strError))
                {
                    ObjXML.LoadXml(strxml.Replace("\"", "'"));
                    string ProcessVersion = ObjXML.ChildNodes.Item(1).Attributes["xmlns"].Value;
                    int n = ProcessVersion.LastIndexOf("/");
                    string ProcessVersionNumber = ProcessVersion.Substring(0, n).ToString() + "/Types";
                    //ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[0].FirstChild.NamespaceURI = "http://processSchema.eGastos/";

                    XmlNode XmlNodeCustom = (ObjXML.ChildNodes[1].ChildNodes[0]);

                    LarXML.Append("<?xml version='1.0' encoding='utf-16'?> ");
                    LarXML.Append("<TaskData xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='" + ProcessVersion + "'>");

                    LarXML.Append("  <Global>");
                    LarXML.Append("    <UltConfiguration xmlns='" + ProcessVersionNumber + "'>");
                    LarXML.Append("      <serverName xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/</serverName>");
                    LarXML.Append("      <appDirectory xmlns='http://processSchema.eGastos/'>eGastos/Pages/</appDirectory>");
                    LarXML.Append("      <pageRequest xmlns='http://processSchema.eGastos/'>frmExpenseRequest.aspx</pageRequest>");
                    LarXML.Append("      <pageApproval xmlns='http://processSchema.eGastos/'>frmExpenseApproval.aspx</pageApproval>");
                    LarXML.Append("      <pageExpenseAccount xmlns='http://processSchema.eGastos/'>frmExpenseAccount.aspx</pageExpenseAccount>");
                    LarXML.Append("      <pageConfirmQuote xmlns='http://processSchema.eGastos/'>frmConfirmQuote.aspx</pageConfirmQuote>");
                    LarXML.Append("      <uRLRequest xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseRequest.aspx</uRLRequest>");
                    LarXML.Append("      <uRLApproval xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseApproval.aspx</uRLApproval>");
                    LarXML.Append("      <uRLExpenseAccount xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseAccount.aspx</uRLExpenseAccount>");
                    LarXML.Append("      <uRLConfirmQuote xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmConfirmQuote.aspx</uRLConfirmQuote>");
                    LarXML.Append("    </UltConfiguration>");
                    if (me.UltApprovalHistory != null)
                    {
                        foreach (UltApprovalHistory obj in me.UltApprovalHistory)
                        {
                            LarXML.Append("    <UltApprovalHistory xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <stepName xmlns='http://processSchema.eGastos/'>" + obj.stepName + "</stepName>");
                            LarXML.Append("      <approverName xmlns='http://processSchema.eGastos/'>" + obj.approverName + "</approverName>");
                            LarXML.Append("      <approverLogin xmlns='http://processSchema.eGastos/'>" + obj.approverLogin + "</approverLogin>");
                            LarXML.Append("      <userEmail xmlns='http://processSchema.eGastos/'>" + obj.userEmail + "</userEmail>");
                            LarXML.Append("      <approveDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.approveDate) + "</approveDate>");
                            LarXML.Append("      <comments xmlns='http://processSchema.eGastos/'>" + obj.comments + "</comments>");
                            LarXML.Append("      <approveStatus xmlns='http://processSchema.eGastos/'>" + obj.approveStatus + "</approveStatus>");
                            LarXML.Append("    </UltApprovalHistory>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltApprovalHistory xmlns='" + ProcessVersionNumber + "'> ");
                        LarXML.Append("      <approveDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</approveDate> ");
                        LarXML.Append("    </UltApprovalHistory>");
                    }

                    if (me.UltApprove != null)
                    {
                        LarXML.Append("    <UltApprove xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("         <approved xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approved.ToString().ToLower() + "</approved>");
                        LarXML.Append("         <approverName xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverName + "</approverName>");
                        LarXML.Append("         <approverLogin xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverLogin + "</approverLogin>");
                        LarXML.Append("         <approverEmail xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverEmail + "</approverEmail>");
                        LarXML.Append("    </UltApprove>");
                    }
                    else
                    {
                        LarXML.Append("    <UltApprove xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("         <approved xmlns='http://processSchema.eGastos/'>false</approved>");
                        LarXML.Append("    </UltApprove>");

                    }

                    if (me.UltExpenseAccount != null)
                    {
                        LarXML.Append("    <UltExpenseAccount xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idExpenseAccount xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.idExpenseAccount + "</idExpenseAccount>");
                        LarXML.Append("      <nationalManagerLogin xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.nationalManagerLogin + "</nationalManagerLogin>");
                        LarXML.Append("      <nationalManagerName xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.nationalManagerName + "</nationalManagerName>");
                        LarXML.Append("      <creditCard xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.creditCard.ToString().ToLower() + "</creditCard>");
                        LarXML.Append("      <totalMiniEvent xmlns='http://processSchema.eGastos/'>" + (double)me.UltExpenseAccount.totalMiniEvent + "</totalMiniEvent>");
                        LarXML.Append("      <totalMeal xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.totalMeal + "</totalMeal>");
                        LarXML.Append("      <totalNationalMeal xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.totalNationalMeal + "</totalNationalMeal>");
                        LarXML.Append("      <overdue xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.overdue.ToString().ToLower() + "</overdue>");
                        LarXML.Append("      <charged xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.charged.ToString().ToLower() + "</charged>");
                        LarXML.Append("    </UltExpenseAccount>");
                    }
                    else
                    {
                        LarXML.Append("    <UltExpenseAccount xmlns='" + ProcessVersionNumber + "'> ");
                        LarXML.Append("          <idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount> ");
                        LarXML.Append("          <creditCard xmlns='http://processSchema.eGastos/'>false</creditCard> ");
                        LarXML.Append("          <totalMiniEvent xmlns='http://processSchema.eGastos/'>0</totalMiniEvent> ");
                        LarXML.Append("          <totalMeal xmlns='http://processSchema.eGastos/'>0</totalMeal> ");
                        LarXML.Append("          <totalNationalMeal xmlns='http://processSchema.eGastos/'>0</totalNationalMeal> ");
                        LarXML.Append("          <overdue xmlns='http://processSchema.eGastos/'>false</overdue> ");
                        LarXML.Append("          <charged xmlns='http://processSchema.eGastos/'>false</charged> ");
                        LarXML.Append("    </UltExpenseAccount> ");

                    }
                    if (me.UltExpenseAccountDetail != null)
                    {
                        foreach (eGastosEntity.Ultimus.UltExpenseAccountDetail obj in me.UltExpenseAccountDetail)
                        {
                            LarXML.Append("    <UltExpenseAccountDetail xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccountDetail + "</idExpenseAccountDetail>");
                            LarXML.Append("      <idExpenseAccount xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccount + "</idExpenseAccount>");
                            LarXML.Append("      <expenseDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.expenseDate) + "</expenseDate>");
                            LarXML.Append("      <idAccount xmlns='http://processSchema.eGastos/'>" + obj.idAccount + "</idAccount>");
                            LarXML.Append("      <amount xmlns='http://processSchema.eGastos/'>" + obj.amount + "</amount>");
                            LarXML.Append("      <invoiceNumber xmlns='http://processSchema.eGastos/'>" + obj.invoiceNumber + "</invoiceNumber>");
                            LarXML.Append("      <place xmlns='http://processSchema.eGastos/'>" + obj.place + "</place>");
                            LarXML.Append("      <numberOfDiners xmlns='http://processSchema.eGastos/'>" + obj.numberOfDiners + "</numberOfDiners>");
                            LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + obj.IVA + "</IVA>");
                            LarXML.Append("      <healthProfessional xmlns='http://processSchema.eGastos/'>" + obj.healthProfessional.ToString().ToLower() + "</healthProfessional>");
                            LarXML.Append("      <discount xmlns='http://processSchema.eGastos/'>" + obj.discount + "</discount>");
                            LarXML.Append("      <hasPAClient xmlns='http://processSchema.eGastos/'>" + obj.hasPAClient.ToString().ToLower() + "</hasPAClient>");
                            LarXML.Append("      <IVATypeId xmlns='http://processSchema.eGastos/'>" + obj.IVATypeId + "</IVATypeId>");
                            LarXML.Append("      <IVATypeName xmlns='http://processSchema.eGastos/'>" + obj.IVATypeName + "</IVATypeName>");
                            LarXML.Append("      <total xmlns='http://processSchema.eGastos/'>" + obj.total + "</total>");
                            LarXML.Append("      <observationId xmlns='http://processSchema.eGastos/'>" + obj.observationId + "</observationId>");
                            LarXML.Append("      <observationName xmlns='http://processSchema.eGastos/'>" + obj.observationName + "</observationName>");
                            LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
                            LarXML.Append("    </UltExpenseAccountDetail>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltExpenseAccountDetail xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("          <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail>");
                        LarXML.Append("          <idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount>");
                        LarXML.Append("          <expenseDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</expenseDate>");
                        LarXML.Append("          <idAccount xmlns='http://processSchema.eGastos/'>0</idAccount>");
                        LarXML.Append("          <amount xmlns='http://processSchema.eGastos/'>0</amount>");
                        LarXML.Append("          <numberOfDiners xmlns='http://processSchema.eGastos/'>0</numberOfDiners>");
                        LarXML.Append("          <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
                        LarXML.Append("          <healthProfessional xmlns='http://processSchema.eGastos/'>false</healthProfessional>");
                        LarXML.Append("          <discount xmlns='http://processSchema.eGastos/'>0</discount>");
                        LarXML.Append("          <hasPAClient xmlns='http://processSchema.eGastos/'>false</hasPAClient>");
                        LarXML.Append("          <total xmlns='http://processSchema.eGastos/'>0</total>");
                        LarXML.Append("          <observationId xmlns='http://processSchema.eGastos/'>0</observationId>");
                        LarXML.Append("          <status xmlns='http://processSchema.eGastos/'>false</status>");
                        LarXML.Append("    </UltExpenseAccountDetail>");
                    }

                    if (me.UltExpenseFlowVariables != null)
                    {
                        LarXML.Append("    <UltExpenseFlowVariables xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <summaryText xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.summaryText + "</summaryText>");
                        LarXML.Append("      <jobFunctionResponsible xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionResponsible + "</jobFunctionResponsible>");
                        LarXML.Append("      <activeDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirAreaGastos.ToString().ToLower() + "</activeDirAreaGastos>");
                        LarXML.Append("      <activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirFinanzasGastos.ToString().ToLower() + "</activeDirFinanzasGastos>");
                        LarXML.Append("      <activeManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeManager.ToString().ToLower() + "</activeManager>");
                        LarXML.Append("      <jobFunctionControlling xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionControlling + "</jobFunctionControlling>");
                        LarXML.Append("      <jobFunctionNationalManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionNationalManager + "</jobFunctionNationalManager>");
                        LarXML.Append("      <jobFunctionAutorizador1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador1 + "</jobFunctionAutorizador1>");
                        LarXML.Append("      <jobFunctionAutorizador2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador2 + "</jobFunctionAutorizador2>");
                        LarXML.Append("      <jobFunctionAutorizador3 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador3 + "</jobFunctionAutorizador3>");
                        LarXML.Append("      <jobFunctionAutorizador4 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador4 + "</jobFunctionAutorizador4>");
                        LarXML.Append("      <jobFunctionController1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController1 + "</jobFunctionController1>");
                        LarXML.Append("      <jobFunctionController2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController2 + "</jobFunctionController2>");
                        LarXML.Append("      <jobFunctionObservador xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionObservador + "</jobFunctionObservador>");
                        LarXML.Append("      <jobFunctionDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionDirAreaGastos + "</jobFunctionDirAreaGastos>");
                        LarXML.Append("      <jobFunctionFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionFinanzasGastos + "</jobFunctionFinanzasGastos>");
                        LarXML.Append("      <activeDirGralGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirGralGastos.ToString().ToLower() + "</activeDirGralGastos>");
                        LarXML.Append("    </UltExpenseFlowVariables>");
                    }
                    else
                    {
                        LarXML.Append("    <UltExpenseFlowVariables xmlns='" + ProcessVersionNumber + "'> ");
                        LarXML.Append("    <activeDirAreaGastos xmlns='http://processSchema.eGastos/'>false</activeDirAreaGastos> ");
                        LarXML.Append("    <activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>false</activeDirFinanzasGastos> ");
                        LarXML.Append("    <activeManager xmlns='http://processSchema.eGastos/'>false</activeManager>");
                        LarXML.Append("    <activeDirGralGastos xmlns='http://processSchema.eGastos/'>false</activeDirGralGastos> ");
                        LarXML.Append("    </UltExpenseFlowVariables> ");
                    }

                    if (me.UltFlobotVariables != null)
                    {
                        LarXML.Append("    <UltFlobotVariables xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <statusAgencyFlobot xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.statusAgencyFlobot + "</statusAgencyFlobot>");
                        LarXML.Append("      <messageErrorAgency xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.messageErrorAgency + "</messageErrorAgency>");
                        LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.status + "</status>");
                        LarXML.Append("      <messageError xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.messageError + "</messageError>");
                        LarXML.Append("    </UltFlobotVariables>");
                    }
                    else
                    {
                        LarXML.Append("    <UltFlobotVariables xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <statusAgencyFlobot xmlns='http://processSchema.eGastos/'>0</statusAgencyFlobot>");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>0</status>");
                        LarXML.Append("    </UltFlobotVariables>");
                    }

                    if (me.UltHotel != null)
                    {
                        foreach (eGastosEntity.Ultimus.UltHotel obj in me.UltHotel)
                        {
                            LarXML.Append("    <UltHotel xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idHotel xmlns='http://processSchema.eGastos/'>" + obj.idHotel + "</idHotel>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <idRated xmlns='http://processSchema.eGastos/'>" + obj.idRated + "</idRated>");
                            LarXML.Append("      <idConsecutive xmlns='http://processSchema.eGastos/'>" + obj.idConsecutive + "</idConsecutive>");
                            LarXML.Append("      <idLegerAccount xmlns='http://processSchema.eGastos/'>" + obj.idLegerAccount + "</idLegerAccount>");
                            LarXML.Append("      <nameLegerAccount xmlns='http://processSchema.eGastos/'>" + obj.nameLegerAccount + "</nameLegerAccount>");
                            LarXML.Append("      <country xmlns='http://processSchema.eGastos/'>" + obj.country + "</country>");
                            LarXML.Append("      <city xmlns='http://processSchema.eGastos/'>" + obj.city + "</city>");
                            LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
                            LarXML.Append("      <checkInDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.checkInDate) + "</checkInDate>");
                            LarXML.Append("      <checkoutDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.checkoutDate) + "</checkoutDate>");
                            LarXML.Append("      <hotelName xmlns='http://processSchema.eGastos/'>" + obj.hotelName + "</hotelName>");
                            LarXML.Append("      <reservation xmlns='http://processSchema.eGastos/'>" + obj.reservation + "</reservation>");
                            LarXML.Append("      <telephone xmlns='http://processSchema.eGastos/'>" + obj.telephone + "</telephone>");
                            LarXML.Append("      <address xmlns='http://processSchema.eGastos/'>" + obj.address + "</address>");
                            LarXML.Append("      <quotedRate xmlns='http://processSchema.eGastos/'>" + obj.quotedRate + "</quotedRate>");
                            LarXML.Append("      <realRate xmlns='http://processSchema.eGastos/'>" + obj.realRate + "</realRate>");
                            LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + obj.IVA + "</IVA>");
                            LarXML.Append("      <hotelTax xmlns='http://processSchema.eGastos/'>" + obj.hotelTax + "</hotelTax>");
                            LarXML.Append("      <otherTaxes xmlns='http://processSchema.eGastos/'>" + obj.otherTaxes + "</otherTaxes>");
                            LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
                            LarXML.Append("      <lineStatus xmlns='http://processSchema.eGastos/'>" + obj.lineStatus + "</lineStatus>");
                            LarXML.Append("      <lineStatusName xmlns='http://processSchema.eGastos/'>" + obj.lineStatusName + "</lineStatusName>");
                            LarXML.Append("    </UltHotel>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltHotel xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idHotel xmlns='http://processSchema.eGastos/'>0</idHotel>");
                        LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("    <idRated xmlns='http://processSchema.eGastos/'>0</idRated>");
                        LarXML.Append("    <idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive>");
                        LarXML.Append("    <idLegerAccount xmlns='http://processSchema.eGastos/'>0</idLegerAccount>");
                        LarXML.Append("    <checkInDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkInDate>");
                        LarXML.Append("    <checkoutDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkoutDate>");
                        LarXML.Append("    <quotedRate xmlns='http://processSchema.eGastos/'>0</quotedRate>");
                        LarXML.Append("    <realRate xmlns='http://processSchema.eGastos/'>0</realRate>");
                        LarXML.Append("    <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
                        LarXML.Append("    <hotelTax xmlns='http://processSchema.eGastos/'>0</hotelTax>");
                        LarXML.Append("    <otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes>");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>false</status>");
                        LarXML.Append("    <lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus>");
                        LarXML.Append("    </UltHotel>");
                    }

                    if (me.UltItinerary != null)
                    {

                        foreach (eGastosEntity.Ultimus.UltItinerary obj in me.UltItinerary)
                        {
                            LarXML.Append("    <UltItinerary xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idItinerary xmlns='http://processSchema.eGastos/'>" + obj.idItinerary + "</idItinerary>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <idConsecutive xmlns='http://processSchema.eGastos/'>" + obj.idConsecutive + "</idConsecutive>");
                            LarXML.Append("      <idLedgerAccount xmlns='http://processSchema.eGastos/'>" + obj.idLedgerAccount + "</idLedgerAccount>");
                            LarXML.Append("      <nameLedgerAccount xmlns='http://processSchema.eGastos/'>" + obj.nameLedgerAccount + "</nameLedgerAccount>");
                            LarXML.Append("      <departureHour xmlns='http://processSchema.eGastos/'>" + obj.departureHour + "</departureHour>");
                            LarXML.Append("      <returnHour xmlns='http://processSchema.eGastos/'>" + obj.returnHour + "</returnHour>");
                            LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
                            LarXML.Append("      <travelType xmlns='http://processSchema.eGastos/'>" + obj.travelType + "</travelType>");
                            LarXML.Append("      <nameTravelType xmlns='http://processSchema.eGastos/'>" + obj.nameTravelType + "</nameTravelType>");
                            LarXML.Append("      <departureCountry xmlns='http://processSchema.eGastos/'>" + obj.departureCountry + "</departureCountry>");
                            LarXML.Append("      <departureCity xmlns='http://processSchema.eGastos/'>" + obj.departureCity + "</departureCity>");
                            LarXML.Append("      <arrivalCountry xmlns='http://processSchema.eGastos/'>" + obj.arrivalCountry + "</arrivalCountry>");
                            LarXML.Append("      <arrivalCity xmlns='http://processSchema.eGastos/'>" + obj.arrivalCity + "</arrivalCity>");
                            LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.departureDate) + "</departureDate>");
                            LarXML.Append("      <arrivalDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.arrivalDate) + "</arrivalDate>");
                            LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
                            LarXML.Append("    </UltItinerary>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltItinerary xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idItinerary xmlns='http://processSchema.eGastos/'>0</idItinerary>");
                        LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("    <idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive>");
                        LarXML.Append("    <idLedgerAccount xmlns='http://processSchema.eGastos/'>0</idLedgerAccount>");
                        LarXML.Append("    <travelType xmlns='http://processSchema.eGastos/'>0</travelType>");
                        LarXML.Append("    <departureDate xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    <arrivalDate xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>false</status>");
                        LarXML.Append("    </UltItinerary>");
                    }

                    if (me.UltItineraryOptions != null)
                    {
                        foreach (UltItineraryOptions obj in me.UltItineraryOptions)
                        {
                            LarXML.Append("    <UltItineraryOptions xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idItineraryOption xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOption + "</idItineraryOption>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <idRate xmlns='http://processSchema.eGastos/'>" + obj.idRate + "</idRate>");
                            LarXML.Append("      <quoteRate xmlns='http://processSchema.eGastos/'>" + obj.quoteRate + "</quoteRate>");
                            LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
                            LarXML.Append("      <confirmed xmlns='http://processSchema.eGastos/'>" + obj.confirmed.ToString().ToLower() + "</confirmed>");
                            LarXML.Append("      <lastDayPurchase xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.lastDayPurchase) + "</lastDayPurchase>");
                            LarXML.Append("    </UltItineraryOptions>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltItineraryOptions xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption>");
                        LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("    <idRate xmlns='http://processSchema.eGastos/'>0</idRate>");
                        LarXML.Append("    <quoteRate xmlns='http://processSchema.eGastos/'>0</quoteRate>");
                        LarXML.Append("    <confirmed xmlns='http://processSchema.eGastos/'>false</confirmed>");
                        LarXML.Append("    <lastDayPurchase xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</lastDayPurchase>");
                        LarXML.Append("    </UltItineraryOptions>");
                    }

                    if (me.UltItineraryOptionsDetail != null)
                    {
                        foreach (UltItineraryOptionsDetail obj in me.UltItineraryOptionsDetail)
                        {
                            LarXML.Append("    <UltItineraryOptionsDetail xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOptionsDetail + "</idItineraryOptionsDetail>");
                            LarXML.Append("      <idItineraryOption xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOption + "</idItineraryOption>");
                            LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
                            LarXML.Append("      <airlineFlight xmlns='http://processSchema.eGastos/'>" + obj.airlineFlight + "</airlineFlight>");
                            LarXML.Append("      <departure xmlns='http://processSchema.eGastos/'>" + obj.departure + "</departure>");
                            LarXML.Append("      <arrival xmlns='http://processSchema.eGastos/'>" + obj.arrival + "</arrival>");
                            LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.departureDate) + "</departureDate>");
                            LarXML.Append("      <arrivalDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.arrivalDate) + "</arrivalDate>");
                            LarXML.Append("      <lapseTime xmlns='http://processSchema.eGastos/'>" + obj.lapseTime + "</lapseTime>");
                            LarXML.Append("    </UltItineraryOptionsDetail>");
                        }
                    }
                    else
                    {
                        LarXML.Append("        <UltItineraryOptionsDetail xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("        <idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>0</idItineraryOptionsDetail>");
                        LarXML.Append("        <idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption>");
                        LarXML.Append("        <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("        <departureDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</departureDate>");
                        LarXML.Append("        <arrivalDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</arrivalDate>");
                        LarXML.Append("        <lapseTime xmlns='http://processSchema.eGastos/'>0</lapseTime>");
                        LarXML.Append("        </UltItineraryOptionsDetail>");
                    }

                    if (me.UltItineraryRate != null)
                    {
                        LarXML.Append("    <UltItineraryRate xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <IdItineraryRate xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.IdItineraryRate + "</IdItineraryRate>");
                        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.idMissionOrder + "</idMissionOrder>");
                        LarXML.Append("      <realRate xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.realRate + "</realRate>");
                        LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.IVA + "</IVA>");
                        LarXML.Append("      <TUA xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.TUA + "</TUA>");
                        LarXML.Append("      <otherTaxes xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.otherTaxes + "</otherTaxes>");
                        LarXML.Append("      <lineStatus xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.lineStatus + "</lineStatus>");
                        LarXML.Append("      <lineStatusName xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.lineStatusName + "</lineStatusName>");
                        LarXML.Append("    </UltItineraryRate>");
                    }
                    else
                    {
                        LarXML.Append("        <UltItineraryRate xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("            <IdItineraryRate xmlns='http://processSchema.eGastos/'>0</IdItineraryRate>");
                        LarXML.Append("            <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("            <realRate xmlns='http://processSchema.eGastos/'>0</realRate>");
                        LarXML.Append("            <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
                        LarXML.Append("            <TUA xmlns='http://processSchema.eGastos/'>0</TUA>");
                        LarXML.Append("            <otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes>");
                        LarXML.Append("            <lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus>");
                        LarXML.Append("        </UltItineraryRate>");
                    }

                    if (me.UltMissionOrder != null)
                    {
                        LarXML.Append("    <UltMissionOrder xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idMissionOrder + "</idMissionOrder>");
                        LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idRequest + "</idRequest>");
                        LarXML.Append("      <idAgencyResponse xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idAgencyResponse + "</idAgencyResponse>");
                        LarXML.Append("      <statusAgencyProcess xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.statusAgencyProcess + "</statusAgencyProcess>");
                        LarXML.Append("      <statusAgencySend xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.statusAgencySend + "</statusAgencySend>");
                        LarXML.Append("      <countAgencyWait xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.countAgencyWait + "</countAgencyWait>");
                        LarXML.Append("      <idAgencyLog xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idAgencyLog + "</idAgencyLog>");
                        LarXML.Append("      <travelId xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.travelId + "</travelId>");
                        LarXML.Append("      <travelName xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.travelName + "</travelName>");
                        LarXML.Append("      <objective xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.objective + "</objective>");
                        LarXML.Append("      <advance xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.advance + "</advance>");
                        LarXML.Append("      <nationalCurrency xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.nationalCurrency + "</nationalCurrency>");
                        LarXML.Append("      <advanceApply xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.advanceApply.ToString().ToLower() + "</advanceApply>");
                        LarXML.Append("      <itinerary xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.itinerary.ToString().ToLower() + "</itinerary>");
                        LarXML.Append("      <hotel xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.hotel.ToString().ToLower() + "</hotel>");
                        if (!string.IsNullOrEmpty(me.UltMissionOrder.comment))
                        {
                            LarXML.Append("      <comment xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.comment + "</comment>");
                        }
                        LarXML.Append("    </UltMissionOrder>");
                    }
                    else
                    {
                        LarXML.Append("      <UltMissionOrder xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
                        LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
                        LarXML.Append("      <idAgencyResponse xmlns='http://processSchema.eGastos/'>0</idAgencyResponse>");
                        LarXML.Append("      <statusAgencyProcess xmlns='http://processSchema.eGastos/'>0</statusAgencyProcess>");
                        LarXML.Append("      <statusAgencySend xmlns='http://processSchema.eGastos/'>0</statusAgencySend>");
                        LarXML.Append("      <countAgencyWait xmlns='http://processSchema.eGastos/'>0</countAgencyWait>");
                        LarXML.Append("      <idAgencyLog xmlns='http://processSchema.eGastos/'>0</idAgencyLog>");
                        LarXML.Append("      <advance xmlns='http://processSchema.eGastos/'>0</advance>");
                        LarXML.Append("      <nationalCurrency xmlns='http://processSchema.eGastos/'>0</nationalCurrency>");
                        LarXML.Append("      <advanceApply xmlns='http://processSchema.eGastos/'>false</advanceApply>");
                        LarXML.Append("      <itinerary xmlns='http://processSchema.eGastos/'>false</itinerary>");
                        LarXML.Append("      <hotel xmlns='http://processSchema.eGastos/'>false</hotel>");
                        LarXML.Append("      </UltMissionOrder>");
                    }

                    if (me.UltPAClient != null)
                    {
                        foreach (eGastosEntity.Ultimus.UltPAClient obj in me.UltPAClient)
                        {
                            LarXML.Append("    <UltPAClient xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccountDetail + "</idExpenseAccountDetail>");
                            LarXML.Append("      <code xmlns='http://processSchema.eGastos/'>" + obj.code + "</code>");
                            LarXML.Append("      <name xmlns='http://processSchema.eGastos/'>" + obj.name + "</name>");
                            LarXML.Append("    </UltPAClient>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltPAClient xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail>");
                        LarXML.Append("    </UltPAClient>");
                    }

                    if (me.UltRequest != null)
                    {
                        LarXML.Append("    <UltRequest xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + me.UltRequest.idRequest + "</idRequest>");
                        LarXML.Append("      <requestDate xmlns='http://processSchema.eGastos/'>" + strHoraAtual + "</requestDate>");
                        if (!string.IsNullOrEmpty(me.UltRequest.companyName))
                        {
                            LarXML.Append("      <companyName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.companyName + "</companyName>");
                        }
                        LarXML.Append("      <companyCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.companyCode + "</companyCode>");
                        LarXML.Append("      <CeCoCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoCode + "</CeCoCode>");
                        LarXML.Append("      <CeCoName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoName + "</CeCoName>");
                        LarXML.Append("      <CeCoMiniCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoMiniCode + "</CeCoMiniCode>");
                        LarXML.Append("      <CeCoMiniName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoMiniName + "</CeCoMiniName>");
                        LarXML.Append("      <isMiniEvent xmlns='http://processSchema.eGastos/'>" + me.UltRequest.isMiniEvent.ToString().ToLower() + "</isMiniEvent>");
                        if (!string.IsNullOrEmpty(me.UltRequest.arrival))
                        {
                            LarXML.Append("      <arrival xmlns='http://processSchema.eGastos/'>" + me.UltRequest.arrival + "</arrival>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.departureDate))
                        {
                            LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.departureDate + "</departureDate>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.returnDate))
                        {
                            LarXML.Append("      <returnDate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.returnDate + "</returnDate>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.PEPElementId))
                        {
                            LarXML.Append("      <PEPElementId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PEPElementId + "</PEPElementId>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.PEPElementName))
                        {
                            LarXML.Append("      <PEPElementName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PEPElementName + "</PEPElementName>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.currencyId))
                        {
                            LarXML.Append("      <currencyId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.currencyId + "</currencyId>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.currencyName))
                        {
                            LarXML.Append("      <currencyName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.currencyName + "</currencyName>");
                        }
                        LarXML.Append("      <exchangeRate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.exchangeRate + "</exchangeRate>");
                        if (!string.IsNullOrEmpty(me.UltRequest.initiatorLogin))
                        {
                            LarXML.Append("      <initiatorLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequest.initiatorLogin + "</initiatorLogin>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.initiatorName))
                        {
                            LarXML.Append("      <initiatorName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.initiatorName + "</initiatorName>");
                        }
                        LarXML.Append("      <PAClientId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PAClientId + "</PAClientId>");
                        LarXML.Append("      <PAClientName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PAClientName + "</PAClientName>");
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleLogin))
                        {
                            LarXML.Append("      <responsibleLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleLogin + "</responsibleLogin>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleName))
                        {
                            LarXML.Append("      <responsibleName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleName + "</responsibleName>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleEmployeeNum))
                        {
                            LarXML.Append("      <responsibleEmployeeNum xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleEmployeeNum + "</responsibleEmployeeNum>");
                        }
                        if (!string.IsNullOrEmpty(me.UltRequest.responsibleUserName))
                        {
                            LarXML.Append("      <responsibleUserName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleUserName + "</responsibleUserName>");
                        }
                        LarXML.Append("      <responsiblePayMethod xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsiblePayMethod + "</responsiblePayMethod>");
                        LarXML.Append("      <pasteur xmlns='http://processSchema.eGastos/'>" + me.UltRequest.pasteur.ToString().ToLower() + "</pasteur>");
                        LarXML.Append("      <areaId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.areaId + "</areaId>");
                        if (!string.IsNullOrEmpty(me.UltRequest.areaText))
                        {
                            LarXML.Append("      <areaText xmlns='http://processSchema.eGastos/'>" + me.UltRequest.areaText + "</areaText>");
                        }
                        LarXML.Append("      <ultimusNumber xmlns='http://processSchema.eGastos/'>" + me.UltRequest.ultimusNumber + "</ultimusNumber>");

                        LarXML.Append("      <type xmlns='http://processSchema.eGastos/'>" + me.UltRequest.type + "</type>");
                        LarXML.Append("      <typeName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.typeName + "</typeName>");
                        LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + me.UltRequest.status + "</status>");
                        LarXML.Append("      <statusName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.statusName + "</statusName>");
                        LarXML.Append("      <salesForce xmlns='http://processSchema.eGastos/'>" + me.UltRequest.salesForce.ToString().ToLower() + "</salesForce>");
                        LarXML.Append("    </UltRequest>");
                    }
                    else
                    {
                        LarXML.Append("    <UltRequest xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
                        LarXML.Append("    <requestDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requestDate>");
                        LarXML.Append("    <companyCode xmlns='http://processSchema.eGastos/'>0</companyCode>");
                        LarXML.Append("    <CeCoCode xmlns='http://processSchema.eGastos/'>0</CeCoCode>");
                        LarXML.Append("    <CeCoMiniCode xmlns='http://processSchema.eGastos/'>0</CeCoMiniCode>");
                        LarXML.Append("    <isMiniEvent xmlns='http://processSchema.eGastos/'>false</isMiniEvent>");
                        LarXML.Append("    <exchangeRate xmlns='http://processSchema.eGastos/'>0</exchangeRate>");
                        LarXML.Append("    <pasteur xmlns='http://processSchema.eGastos/'>false</pasteur>");
                        LarXML.Append("    <areaId xmlns='http://processSchema.eGastos/'>0</areaId>");
                        LarXML.Append("    <ultimusNumber xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    <type xmlns='http://processSchema.eGastos/'>0</type>");
                        LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>0</status>");
                        LarXML.Append("    <salesForce xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
                        LarXML.Append("    </UltRequest>");
                    }
                    if (me.UltRequester != null)
                    {
                        LarXML.Append("    <UltRequester xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("      <requesterName xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterName + "</requesterName>");
                        LarXML.Append("      <requesterLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterLogin + "</requesterLogin>");
                        LarXML.Append("      <requesterEmail xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterEmail + "</requesterEmail>");
                        LarXML.Append("      <requesterDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(me.UltRequester.requesterDate) + "</requesterDate>");
                        LarXML.Append("    </UltRequester>");
                    }
                    else
                    {
                        LarXML.Append("    <UltRequester xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <requesterDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requesterDate>");
                        LarXML.Append("    </UltRequester>");
                    }

                    if (me.UltResponsible != null)
                    {
                        LarXML.Append("    <UltResponsible xmlns='" + ProcessVersionNumber + "' >");
                        LarXML.Append("      <responsibleName xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleName + "</responsibleName>");
                        LarXML.Append("      <responsibleLogin xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleLogin + "</responsibleLogin>");
                        LarXML.Append("      <responsibleEmail xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleEmail + "</responsibleEmail>");
                        LarXML.Append("    </UltResponsible>");
                    }
                    else
                    {
                        LarXML.Append("    <UltResponsible xmlns='" + ProcessVersionNumber + "' />");
                    }

                    if (me.UltSAPResponse != null)
                    {
                        foreach (UltSAPResponse obj in me.UltSAPResponse)
                        {
                            LarXML.Append("    <UltSAPResponse xmlns='" + ProcessVersionNumber + "'>");
                            LarXML.Append("      <idResponse xmlns='http://processSchema.eGastos/'>" + obj.idResponse + "</idResponse>");
                            LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + obj.idRequest + "</idRequest>");
                            LarXML.Append("      <docNumber xmlns='http://processSchema.eGastos/'>" + obj.docNumber + "</docNumber>");
                            LarXML.Append("      <company xmlns='http://processSchema.eGastos/'>" + obj.company + "</company>");
                            LarXML.Append("      <year xmlns='http://processSchema.eGastos/'>" + obj.year + "</year>");
                            LarXML.Append("      <type xmlns='http://processSchema.eGastos/'>" + obj.type + "</type>");
                            LarXML.Append("    </UltSAPResponse>");
                        }
                    }
                    else
                    {
                        LarXML.Append("    <UltSAPResponse xmlns='" + ProcessVersionNumber + "'>");
                        LarXML.Append("    <idResponse xmlns='http://processSchema.eGastos/'>0</idResponse>");
                        LarXML.Append("    <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
                        LarXML.Append("    <docNumber xmlns='http://processSchema.eGastos/'>0</docNumber>");
                        LarXML.Append("    <company xmlns='http://processSchema.eGastos/'>0</company>");
                        LarXML.Append("    <year xmlns='http://processSchema.eGastos/'>0</year>");
                        LarXML.Append("    </UltSAPResponse>");
                    }

                    LarXML.Append("        <Cancel xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <URLAttach xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <DaysOutOfCountry xmlns='" + ProcessVersionNumber + "'>0</DaysOutOfCountry>");
                    LarXML.Append("        <WSAgencyName xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <WSAgencyURL xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <WSChangeRequestName xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <WSChangeRequestURL xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <EmailAgency xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <Travel_Nacional xmlns='" + ProcessVersionNumber + "'>false</Travel_Nacional>");
                    LarXML.Append("        <Travel_Internacional xmlns='" + ProcessVersionNumber + "'>false</Travel_Internacional>");
                    LarXML.Append("        <Travel_Domestic xmlns='" + ProcessVersionNumber + "'>false</Travel_Domestic>");
                    LarXML.Append("        <UltGlobal xmlns='" + ProcessVersionNumber + "'>");
                    LarXML.Append("          <idSession xmlns='http://processSchema.eGastos/'>0</idSession>");
                    LarXML.Append("        </UltGlobal>");
                    LarXML.Append("        <ExpenseAccountTotal xmlns='" + ProcessVersionNumber + "'>");
                    LarXML.Append("          <WithoutIVA>0</WithoutIVA>");
                    LarXML.Append("          <IVA>0</IVA>");
                    LarXML.Append("          <Amount>0</Amount>");
                    LarXML.Append("        </ExpenseAccountTotal>");
                    LarXML.Append("        <delayExpenseAccount xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
                    LarXML.Append("        <completionExpenseAccount xmlns='" + ProcessVersionNumber + "'>5</completionExpenseAccount>");
                    LarXML.Append("      </Global>");
                    LarXML.Append("      <StepSchemaUltApprovalHistory>");
                    LarXML.Append("        <approveDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</approveDate>");
                    LarXML.Append("      </StepSchemaUltApprovalHistory>");
                    LarXML.Append("      <SYS_PROCESSATTACHMENTS />");
                    LarXML.Append("    </TaskData>");
                    //-------------
                    strxml1 = LarXML.ToString();
                    strError = "";
                    bolResultado = ult_obj.LaunchIncident(fd.UserLogin, summary, "Incidente criado por: ", false, 9, strxml1, true, out intIncident, out strError);
                }
                nIncident = intIncident;
                if (strError != "")
                {
                    msgError = strError;
                    return intIncident;
                }
            }
            catch (Exception ex)
            {
                msgError = strError = ex.Message;
                nIncident = 0;
                return intIncident;
            }
            return intIncident;
        }

        private static string ToXMLDateFormat(System.DateTime? dti)
        {
            DateTime dt = new DateTime();
            string returnData = "0001-01-01T00:00:00";
            if (dti == null)
            {
                returnData = "0001-01-01T00:00:00";
            }
            else
            {
                dt = dti.Value;
                returnData = dt.Year.ToString() + "-" + (dt.Month < 10 ? "0" + dt.Month.ToString() : dt.Month.ToString()) + "-" + (dt.Day < 10 ? "0" + dt.Day.ToString() : dt.Day.ToString()) + "T" + (dt.Hour < 10 ? "0" + dt.Hour.ToString() : dt.Hour.ToString()) + ":" + (dt.Minute < 10 ? "0" + dt.Minute.ToString() : dt.Minute.ToString()) + ":" + (dt.Second < 10 ? "0" + dt.Second.ToString() : dt.Second.ToString());
            }
            return returnData;
        }

        private ExpenseAccountClient expAccClient = new ExpenseAccountClient();
        private string generaExpenseAcc(eGastosEntity.Ultimus.UltRequest ultReq, eGastosEntity.Ultimus.UltExpenseAccount ultExpAcc, eGastosEntity.Ultimus.UltExpenseAccountDetail[] ultExpAccDetLst, eGastosEntity.Ultimus.UltPAClient[] ultPACliLst)
        {

            expAccClient.initiateVariables(ultReq.requestDate, ultReq.companyName, ultReq.companyCode, ultReq.CeCoCode, ultReq.CeCoMiniCode,
                ultReq.CeCoMiniName, ultReq.isMiniEvent, ultReq.arrival, ultReq.departureDate, ultReq.requestDate.ToString(), ultReq.PEPElementId,
                ultReq.PEPElementName, ultReq.currencyId, ultReq.currencyName, ultReq.initiatorLogin, ultReq.initiatorName, ultReq.responsibleLogin,
                ultReq.responsibleName, ultReq.responsibleEmployeeNum, ultReq.responsibleUserName, ultReq.pasteur, ultReq.areaId,
                ultReq.areaText, (bool)ultReq.salesForce, ultExpAcc.nationalManagerLogin, ultExpAcc.nationalManagerName, ultExpAcc.creditCard);
            if (ultExpAccDetLst != null)
            {
                foreach (eGastosEntity.Ultimus.UltExpenseAccountDetail ultExpAccDet in ultExpAccDetLst)
                {
                    expAccClient.createExpenseAccountDetail(ultExpAccDet.idExpenseAccountDetail, ultExpAccDet.expenseDate, (int)ultExpAccDet.idAccount,
                    ultExpAccDet.accountName, ultExpAccDet.amount, ultExpAccDet.invoiceNumber, ultExpAccDet.place, ultExpAccDet.numberOfDiners,
                    ultExpAccDet.IVA, ultExpAccDet.healthProfessional, ultExpAccDet.hasPAClient, ultExpAccDet.total, ultExpAccDet.observationId,
                    ultExpAccDet.observationName);
                }

            }
            if (ultPACliLst != null)
            {
                foreach (eGastosEntity.Ultimus.UltPAClient ultPACli in ultPACliLst)
                {
                    expAccClient.createPAClient(ultPACli.idExpenseAccountDetail, ultPACli.code, ultPACli.name);
                }
            }
            return expAccClient.validateAndSaveRequest(0);
            //return expAccClient.getUltExpenseFlowVariables();

        }
        private MissionOrderClient misOrdClient = new MissionOrderClient();
        private string generaMissionOrder(eGastosEntity.Ultimus.UltRequest ultReq, eGastosEntity.Ultimus.UltMissionOrder ultMisOrd, eGastosEntity.Ultimus.UltItinerary[] ultItiLst, eGastosEntity.Ultimus.UltHotel[] ultHotLst)
        {

            misOrdClient.initiateVariables(ultReq.requestDate, ultReq.companyName, ultReq.companyCode, ultReq.CeCoCode, ultReq.arrival,
                ultReq.departureDate, ultReq.returnDate, ultReq.PEPElementId, ultReq.PEPElementName, ultReq.currencyId, ultReq.currencyName,
                ultReq.initiatorLogin, ultReq.initiatorName, ultReq.responsibleLogin, ultReq.responsibleName, ultReq.responsibleEmployeeNum,
                ultReq.responsibleUserName, ultReq.pasteur, ultReq.areaId, ultReq.areaText, (bool)ultReq.salesForce, ultMisOrd.travelId,
                ultMisOrd.travelName, ultMisOrd.objective, ultMisOrd.advance, ultMisOrd.comment);
            if (ultItiLst != null)
            {
                foreach (eGastosEntity.Ultimus.UltItinerary ultIti in ultItiLst)
                {
                    misOrdClient.createItinerary(ultIti.idItinerary, ultIti.idLedgerAccount, ultIti.nameLedgerAccount, ultIti.departureHour,
                        ultIti.returnHour, ultIti.observations, ultIti.travelType, ultIti.nameTravelType, ultIti.departureCountry, ultIti.departureCity,
                        ultIti.arrivalCountry, ultIti.arrivalCity, ultIti.departureDate, ultIti.arrivalDate);
                }
            }
            if (ultHotLst != null)
            {
                foreach (eGastosEntity.Ultimus.UltHotel ultHot in ultHotLst)
                {
                    misOrdClient.createHotel(ultHot.idHotel, ultHot.idLegerAccount, ultHot.nameLegerAccount, ultHot.country, ultHot.city,
                        ultHot.observations, ultHot.checkInDate, ultHot.checkoutDate);
                }
            }
            return misOrdClient.validateAndSaveRequest();
        }


    }
}
