using System;
using System.ComponentModel;
using System.Web.Services;
using System.Xml;
using eGastosEntity.Ultimus;
using eGastosWS.ExpenseAccountServiceReference;
using eGastosWS.MissionOrderServiceReference;
using eGastosWS.util;
using eGastosWS.WSeGastosPasteur;
using eGastosWS.WSeGastosPharma;
using System.Text;


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
        private WSeGastosPharma.eGastos_Pharma_BC ult_objPharma = null; // Incluir
        private WSeGastosPasteur.eGastos_Pasteur_BC ult_objPasteur = null; // Incluir
        public string msgError = "";
        #endregion
        private Mapeo mapeo = new Mapeo();
        private eGastosWS.Debug.Generate ge = new eGastosWS.Debug.Generate();
        private Entity.FilterData fd = new Entity.FilterData();
        private XmlNode XMLNodeUltExpenseAccount = null, XMLNodeUltExpenseFlowVariables = null, XMLNodeUltRequest = null; // Incluir

        [WebMethod]
        public int ExpensesAccountRequest(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        {
             //Descomentar para publicar - Marcio Nakamura
            String msgMO = generaExpenseAcc(me.UltRequest, me.UltExpenseAccount, me.UltExpenseAccountDetail, me.UltPAClient, false, 0);
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

            // DEBUG
            //string error = "";
            //Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            //me = ge.me1(ref fd);

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

             //Descomentar para publicar - Marcio Nakamura
            }
            error = msgMO;
            return 0;
        }

        [WebMethod]
        public int MissionOrderRequest(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        //public int MissionOrderRequest()
        {
            //// DEBUG
            //string error = "";
            //Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            //me = ge.me1(ref fd);

            // Descomentar para publicar - Marcio Nakamura
            //String msgMO = generaMissionOrder(me.UltRequest, me.UltMissionOrder, me.UltItinerary, me.UltHotel, false, 0);
            //if (string.IsNullOrEmpty(msgMO))
            //{
            //    me.UltExpenseFlowVariables = mapeo.MapperData<eGastosWS.MissionOrderServiceReference.UltExpenseFlowVariables,
            //        eGastosEntity.Ultimus.UltExpenseFlowVariables>(misOrdClient.getUltExpenseFlowVariables());
            //    me.UltRequest = mapeo.MapperData<eGastosWS.MissionOrderServiceReference.UltRequest,
            //        eGastosEntity.Ultimus.UltRequest>(misOrdClient.getUltRequest());
            //    me.UltMissionOrder = mapeo.MapperData<eGastosWS.MissionOrderServiceReference.UltMissionOrder,
            //        eGastosEntity.Ultimus.UltMissionOrder>(misOrdClient.getUltMissionOrder());

            //    me.UltItinerary = mapeo.MapperDataList<eGastosWS.MissionOrderServiceReference.UltItinerary,
            //        eGastosEntity.Ultimus.UltItinerary>(misOrdClient.getUltItineraryList());
            //    me.UltHotel = mapeo.MapperDataList<eGastosWS.MissionOrderServiceReference.UltHotel,
            //        eGastosEntity.Ultimus.UltHotel>(misOrdClient.getUltHotelList());

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

            // Descomentar para publicar - Marcio Nakamura
            //}
            //error = msgMO;
            //return 0;
        }

        [WebMethod]
        public Entity.MasterEntity LoadMissionOrderApproval(Entity.FilterData fd)
        //public Entity.MasterEntity LoadMissionOrderApproval()
        {
            // DEBUG
            //string error = "";
            //Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            //me = ge.me1(ref fd);

            string xmlstr = getUltimusXML(fd);
            //------------------------------------------------------------------
            populateEntity(fd, ref MasterEntity, xmlstr);
            return MasterEntity;
        }

        [WebMethod]
        public int SendMissionOrderApproval(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        //public int SendMissionOrderApproval()
        {
            //DEBUG
            //Entity.FilterData fd = new Entity.FilterData();
            //Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            //string error = null;
            //me = ge.me1(ref fd);
            //// EndDebug
            ////------------------------------------------------------------------
            //Comentario


            // Descomentar para publicar - Marcio Nakamura
            //String msgMO = generaMissionOrder(me.UltRequest, me.UltMissionOrder, me.UltItinerary, me.UltHotel, true, fd.IncidentNumber);
            //if (msgMO.ToLower() == "true")
            //{
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

            for (int i = nodeAHFirstIndex; (i < (nodeCountAH + nodeAHLastIndex) + 1) && (a < me.UltApprovalHistory.Length); i++)
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

            XmlNode XmlNodeCustom = (MOApprovalXML.ChildNodes[1].ChildNodes[0]);

            int intIncident = fd.IncidentNumber;
            string summary = me.UltExpenseFlowVariables.summaryText;
            string strError = "";

            if (fd.isPasteur)
            {
                ult_objPasteur = new eGastos_Pasteur_BC();
                ult_objPasteur.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, MOApprovalXML.InnerXml, true, out strError);
            }
            else
            {
                ult_objPharma = new eGastos_Pharma_BC();
                ult_objPharma.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, MOApprovalXML.InnerXml, true, out strError);
            }
            error = strError;
            return 0;

            // Descomentar para publicar - Marcio Nakamura
            //}
            //        else {
            //            error = msgMO;
            //            return 0;
            //        }

        }

        [WebMethod]
        public Entity.MasterEntity LoadExpensesAccountApproval(Entity.FilterData fd)
        {
            string xmlstr = getUltimusXML(fd);
            //------------------------------------------------------------------
            populateEntity(fd, ref MasterEntity, xmlstr);
            return MasterEntity;
        }

        [WebMethod]
        public Entity.MasterEntity LoadReviewData(Entity.FilterData fd)
        {
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

            // Descomentar para publicar - Marcio Nakamura
            //String msgMO = generaExpenseAcc(me.UltRequest, me.UltExpenseAccount, me.UltExpenseAccountDetail, me.UltPAClient, true, fd.IncidentNumber);
            //if (msgMO.ToLower() == "true")
            //{
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

            XmlNode XmlNodeCustom = (MOApprovalXML.ChildNodes[1].ChildNodes[0]);

            int intIncident = fd.IncidentNumber;
            string summary = "";
            string strError = "";

            if (fd.isPasteur)
            {
                ult_objPasteur = new eGastos_Pasteur_BC();
                ult_objPasteur.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, MOApprovalXML.InnerXml, true, out strError);
            }
            else
            {
                ult_objPharma = new eGastos_Pharma_BC();
                ult_objPharma.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, MOApprovalXML.InnerXml, true, out strError);
            }
            error = strError;
            return 0;

            // Descomentar para publicar - Marcio Nakamura
            //}
            //else {
            //    error = msgMO;
            //    return 0;
            //}



        }

        private string getUltimusXML(Entity.FilterData fd)
        {
            string XMLSchemaData = "";

            if (fd.isPasteur)// Call Pasteur XML schema
            {
                WSeGastosPasteur.eGastos_Pasteur_BC wsPasteur = new WSeGastosPasteur.eGastos_Pasteur_BC();
                wsPasteur.GetTaskInformation(fd.UserLogin, fd.IncidentNumber, fd.StepName, out PasteurSchemaFile, out XMLSchemaData, out msgError);
            }
            else //Call Pharma XML Schema
            {
                WSeGastosPharma.eGastos_Pharma_BC wsPHarma = new WSeGastosPharma.eGastos_Pharma_BC();
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
                        me.UltExpenseAccount.debitCard = XmlConvert.ToBoolean(NodeUltExpenseAccount["creditCard"].InnerText.ToString());
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
            WSeGastosPasteur.eGastos_Pasteur_BC ult_obj = new eGastos_Pasteur_BC();
            WSeGastosPasteur.SchemaFile[] schemas;


            // REQUEST  DATA (rd)           
            string strHoraAtual = ToXMLDateFormat(DateTime.Now);
            string xmlStepSchemaUltHotel = "<idHotel xmlns='http://processSchema.eGastos/'>0</idHotel><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idRated xmlns='http://processSchema.eGastos/'>0</idRated><idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive><idLegerAccount xmlns='http://processSchema.eGastos/'>0</idLegerAccount><nameLegerAccount xmlns='http://processSchema.eGastos/'>asd</nameLegerAccount><country xmlns='http://processSchema.eGastos/'>asd</country><city xmlns='http://processSchema.eGastos/'>asd</city><observations xmlns='http://processSchema.eGastos/'>asd</observations><checkInDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkInDate><checkoutDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkoutDate><hotelName xmlns='http://processSchema.eGastos/'>asd</hotelName><reservation xmlns='http://processSchema.eGastos/'>asd</reservation><telephone xmlns='http://processSchema.eGastos/'>asd</telephone><address xmlns='http://processSchema.eGastos/'>asd</address><quotedRate xmlns='http://processSchema.eGastos/'>0</quotedRate><realRate xmlns='http://processSchema.eGastos/'>0</realRate><IVA xmlns='http://processSchema.eGastos/'>0</IVA><hotelTax xmlns='http://processSchema.eGastos/'>0</hotelTax><otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes><status xmlns='http://processSchema.eGastos/'>true</status><lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus><lineStatusName xmlns='http://processSchema.eGastos/'>asd</lineStatusName>";
            string xmlStepSchemaUltApprovalHistory = "<stepName xmlns='http://processSchema.eGastos/'></stepName><approverName xmlns='http://processSchema.eGastos/'></approverName><approverLogin xmlns='http://processSchema.eGastos/'></approverLogin><userEmail xmlns='http://processSchema.eGastos/'></userEmail><approveDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</approveDate><comments xmlns='http://processSchema.eGastos/'></comments><approveStatus xmlns='http://processSchema.eGastos/'></approveStatus>";
            string xmlStepSchemaUltMissionOrder = "<idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idRequest xmlns='http://processSchema.eGastos/'>0</idRequest><idAgencyResponse xmlns='http://processSchema.eGastos/'>0</idAgencyResponse><statusAgencyProcess xmlns='http://processSchema.eGastos/'>0</statusAgencyProcess><statusAgencySend xmlns='http://processSchema.eGastos/'>0</statusAgencySend><countAgencyWait xmlns='http://processSchema.eGastos/'>0</countAgencyWait><idAgencyLog xmlns='http://processSchema.eGastos/'>0</idAgencyLog><travelId xmlns='http://processSchema.eGastos/'></travelId><travelName xmlns='http://processSchema.eGastos/'></travelName><objective xmlns='http://processSchema.eGastos/'></objective><advance xmlns='http://processSchema.eGastos/'>0</advance><nationalCurrency xmlns='http://processSchema.eGastos/'>0</nationalCurrency><advanceApply xmlns='http://processSchema.eGastos/'>false</advanceApply><itinerary xmlns='http://processSchema.eGastos/'>false</itinerary><hotel xmlns='http://processSchema.eGastos/'>false</hotel><comment xmlns='http://processSchema.eGastos/'></comment><exceededAdvance xmlns='http://processSchema.eGastos/'>false</exceededAdvance><missionOrderType xmlns='http://processSchema.eGastos/'>0</missionOrderType><missionOrderTypeText xmlns='http://processSchema.eGastos/'>0</missionOrderTypeText><advanceAndDebitCard xmlns='http://processSchema.eGastos/'>false</advanceAndDebitCard>";
            string xmlStepSchemaUltGetThere = "<idGetThere xmlns='http://processSchema.eGastos/'>0</idGetThere><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><conceptId xmlns='http://processSchema.eGastos/'>0</conceptId><conceptText xmlns='http://processSchema.eGastos/'> </conceptText><lowCost xmlns='http://processSchema.eGastos/'>false</lowCost><justification xmlns='http://processSchema.eGastos/'> </justification><cheapestRate xmlns='http://processSchema.eGastos/'> </cheapestRate><outPolitic xmlns='http://processSchema.eGastos/'>false</outPolitic><outPoliticMessage xmlns='http://processSchema.eGastos/'> </outPoliticMessage>";
            string xmlUltItinerary = "<idItinerary xmlns='http://processSchema.eGastos/'>0</idItinerary><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive><idLedgerAccount xmlns='http://processSchema.eGastos/'>0</idLedgerAccount><nameLedgerAccount  xmlns='http://processSchema.eGastos/'> </nameLedgerAccount><departureHour  xmlns='http://processSchema.eGastos/'>00:00</departureHour><returnHour  xmlns='http://processSchema.eGastos/'>00:00</returnHour><observations  xmlns='http://processSchema.eGastos/'> </observations><travelType xmlns='http://processSchema.eGastos/'>0</travelType><nameTravelType  xmlns='http://processSchema.eGastos/'> </nameTravelType><departureCountry  xmlns='http://processSchema.eGastos/'> </departureCountry><departureCity  xmlns='http://processSchema.eGastos/'> </departureCity><arrivalCountry  xmlns='http://processSchema.eGastos/'> </arrivalCountry><arrivalCity  xmlns='http://processSchema.eGastos/'> </arrivalCity><departureDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</departureDate><arrivalDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</arrivalDate><status xmlns='http://processSchema.eGastos/'>false</status>";
            string xmlUltItineraryOptions = "<idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idRate xmlns='http://processSchema.eGastos/'>0</idRate><quoteRate xmlns='http://processSchema.eGastos/'>0</quoteRate><observations xmlns='http://processSchema.eGastos/'> </observations><confirmed xmlns='http://processSchema.eGastos/'>false</confirmed><lastDayPurchase xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</lastDayPurchase>";
            string xmlUltItineraryOptionsDetail = "<idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>0</idItineraryOptionsDetail><idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><airlineFlight xmlns='http://processSchema.eGastos/'> </airlineFlight><departure xmlns='http://processSchema.eGastos/'> </departure><arrival xmlns='http://processSchema.eGastos/'> </arrival><departureDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</departureDate><arrivalDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</arrivalDate><lapseTime xmlns='http://processSchema.eGastos/'>0</lapseTime>";
            string xmlUltPAClient = "<idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail><code xmlns='http://processSchema.eGastos/'> </code><name xmlns='http://processSchema.eGastos/'> </name>";
            string xmlUltExpenseAccountDetail = "<idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail><idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount><expenseDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</expenseDate><idAccount xmlns='http://processSchema.eGastos/'>0</idAccount><accountName xmlns='http://processSchema.eGastos/'> </accountName><amount xmlns='http://processSchema.eGastos/'>0</amount><invoiceNumber xmlns='http://processSchema.eGastos/'> </invoiceNumber><place xmlns='http://processSchema.eGastos/'> </place><numberOfDiners xmlns='http://processSchema.eGastos/'>0</numberOfDiners><IVA xmlns='http://processSchema.eGastos/'>0</IVA><healthProfessional xmlns='http://processSchema.eGastos/'>false</healthProfessional><discount xmlns='http://processSchema.eGastos/'>0</discount><hasPAClient xmlns='http://processSchema.eGastos/'>false</hasPAClient><IVATypeId xmlns='http://processSchema.eGastos/'> </IVATypeId><IVATypeName xmlns='http://processSchema.eGastos/'> </IVATypeName><total xmlns='http://processSchema.eGastos/'>0</total><observationId xmlns='http://processSchema.eGastos/'>0</observationId><observationName xmlns='http://processSchema.eGastos/'> </observationName><idXml xmlns='http://processSchema.eGastos/'>0</idXml><amountCFDI xmlns='http://processSchema.eGastos/'>0</amountCFDI><ivaCFDI xmlns='http://processSchema.eGastos/'>0</ivaCFDI><idExtract xmlns='http://processSchema.eGastos/'>0</idExtract><amountExtract xmlns='http://processSchema.eGastos/'>0</amountExtract><conciliated xmlns='http://processSchema.eGastos/'>false</conciliated><strike xmlns='http://processSchema.eGastos/'>false</strike><status xmlns='http://processSchema.eGastos/'>false</status>";

            int nodeCountUI = 0, nodeUILastIndex = 0, nodeUIFirstIndex = 0;
            int nodeCountUIO = 0, nodeUIOLastIndex = 0, nodeUIOFirstIndex = 0;
            int nodeCountUIOD = 0, nodeUIODLastIndex = 0, nodeUIODFirstIndex = 0;
            int nodeCountUPAC = 0, nodeUPACLastIndex = 0, nodeUPACFirstIndex = 0;
            int nodeCountUEAD = 0, nodeUEADLastIndex = 0, nodeUEADFirstIndex = 0;

            string summary = me.UltExpenseFlowVariables.summaryText;
            string strError = "";
            string strxml;

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

                    #region StepSchemaUltApprovalHistory
                    ObjXML.ChildNodes[1].ChildNodes[1].InnerXml = xmlStepSchemaUltApprovalHistory;
                    foreach (XmlNode SSUAH in ObjXML.ChildNodes[1].ChildNodes[1])
                    {
                        if (SSUAH.Name == "approveDate") { SSUAH.InnerText = ToXMLDateFormat(me.UltApprovalHistory[0].approveDate); }
                        if (SSUAH.Name == "approverLogin") { SSUAH.InnerText = me.UltApprovalHistory[0].approverLogin; }
                        if (SSUAH.Name == "approverName") { SSUAH.InnerText = me.UltApprovalHistory[0].approverName; }
                        if (SSUAH.Name == "approveStatus") { SSUAH.InnerText = me.UltApprovalHistory[0].approveStatus; }
                        if (SSUAH.Name == "comments") { SSUAH.InnerText = me.UltApprovalHistory[0].comments; }
                        if (SSUAH.Name == "stepName") { SSUAH.InnerText = me.UltApprovalHistory[0].stepName; }
                        if (SSUAH.Name == "userEmail") { SSUAH.InnerText = me.UltApprovalHistory[0].userEmail; }
                    };
                    #endregion

                    #region StepSchemaUltMissionOrder
                    if ((me.UltMissionOrder != null) && (me.UltRequest.type < 3))
                    {
                        ObjXML.ChildNodes[1].ChildNodes[2].InnerXml = xmlStepSchemaUltMissionOrder;
                        foreach (XmlNode SSUMO in ObjXML.ChildNodes[1].ChildNodes[2])
                        {
                            if (SSUMO.Name == "advance") { SSUMO.InnerText = me.UltMissionOrder.advance.ToString(); }
                            if (SSUMO.Name == "advanceAndDebitCard") { SSUMO.InnerText = me.UltMissionOrder.advanceAndDebitCard.ToString().ToLower(); }
                            if (SSUMO.Name == "advanceApply") { SSUMO.InnerText = me.UltMissionOrder.advanceApply.ToString().ToLower(); }
                            if (SSUMO.Name == "comment") { SSUMO.InnerText = me.UltMissionOrder.comment; }
                            if (SSUMO.Name == "countAgencyWait") { SSUMO.InnerText = me.UltMissionOrder.countAgencyWait.ToString(); }
                            if (SSUMO.Name == "exceededAdvance") { SSUMO.InnerText = me.UltMissionOrder.exceededAdvance.ToString().ToLower(); }
                            if (SSUMO.Name == "hotel") { SSUMO.InnerText = me.UltMissionOrder.hotel.ToString().ToLower(); }
                            if (SSUMO.Name == "idAgencyLog") { SSUMO.InnerText = me.UltMissionOrder.idAgencyLog.ToString(); }
                            if (SSUMO.Name == "idAgencyResponse") { SSUMO.InnerText = me.UltMissionOrder.idAgencyResponse.ToString(); }
                            if (SSUMO.Name == "idMissionOrder") { SSUMO.InnerText = me.UltMissionOrder.idMissionOrder.ToString(); }
                            if (SSUMO.Name == "idRequest") { SSUMO.InnerText = me.UltMissionOrder.idRequest.ToString(); }
                            if (SSUMO.Name == "itinerary") { SSUMO.InnerText = me.UltMissionOrder.itinerary.ToString().ToLower(); }
                            if (SSUMO.Name == "missionOrderType") { SSUMO.InnerText = me.UltMissionOrder.missionOrderType.ToString(); }
                            if (SSUMO.Name == "missionOrderTypeText") { SSUMO.InnerText = me.UltMissionOrder.missionOrderTypeText; }
                            if (SSUMO.Name == "nationalCurrency") { SSUMO.InnerText = me.UltMissionOrder.nationalCurrency.ToString(); }
                            if (SSUMO.Name == "objective") { SSUMO.InnerText = me.UltMissionOrder.objective; }
                            if (SSUMO.Name == "statusAgencyProcess") { SSUMO.InnerText = me.UltMissionOrder.statusAgencyProcess.ToString(); }
                            if (SSUMO.Name == "statusAgencySend") { SSUMO.InnerText = me.UltMissionOrder.statusAgencySend.ToString(); }
                            if (SSUMO.Name == "travelId") { SSUMO.InnerText = me.UltMissionOrder.travelId; }
                            if (SSUMO.Name == "travelName") { SSUMO.InnerText = me.UltMissionOrder.travelName; }
                        }
                    }
                    #endregion

                    #region StepSchemaUltGetThere
                    if ((me.UltGetThere != null))
                    {
                        ObjXML.ChildNodes[1].ChildNodes[3].InnerXml = xmlStepSchemaUltGetThere;
                        foreach (XmlNode SSUGT in ObjXML.ChildNodes[1].ChildNodes[3])
                        {
                            if (SSUGT.Name == "idGetThere") { SSUGT.InnerText = me.UltGetThere.idGetThere.ToString(); }
                            if (SSUGT.Name == "idMissionOrder") { SSUGT.InnerText = me.UltGetThere.idMissionOrder.ToString(); }
                            if (SSUGT.Name == "conceptId") { SSUGT.InnerText = me.UltGetThere.conceptId.ToString(); }
                            if (SSUGT.Name == "conceptText") { SSUGT.InnerText = me.UltGetThere.conceptText; }
                            if (SSUGT.Name == "lowCost") { SSUGT.InnerText = me.UltGetThere.lowCost.ToString().ToLower(); }
                            if (SSUGT.Name == "justification") { SSUGT.InnerText = me.UltGetThere.justification; }
                            if (SSUGT.Name == "cheapestRate") { SSUGT.InnerText = me.UltGetThere.cheapestRate; }
                            if (SSUGT.Name == "outPolitic") { SSUGT.InnerText = me.UltGetThere.outPolitic.ToString().ToLower(); }
                            if (SSUGT.Name == "outPoliticMessage") { SSUGT.InnerText = me.UltGetThere.outPoliticMessage; }
                        }
                    }
                    #endregion

                    #region UltItinerary Node Create
                    int nodeCountItinerary = 0;
                    if ((me.UltItinerary != null) && (me.UltItinerary.Length > 1) && (me.UltRequest.type < 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltItinerary")
                            {
                                if (nodeCountItinerary == 0)
                                {
                                    nodeCountItinerary++;
                                    nodeUIFirstIndex = nodeCountItinerary;
                                }
                                else
                                {
                                    nodeUILastIndex = nodeCountItinerary;
                                }
                                nodeCountUI++;
                            }
                            nodeCountItinerary++;
                        }

                        XmlElement xmlUltItineraryElement = ObjXML.CreateElement("UltItinerary", ProcessVersionNumber);
                        for (int i = 0; i < me.UltItinerary.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltItineraryElement.InnerXml = xmlUltItinerary;
                            }
                            else
                            {
                                xmlUltItineraryElement.InnerXml = xmlUltItinerary;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltItineraryElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUILastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltItineraryOptions Node Create
                    int nodeCountUltItineraryOptions = 0;
                    if ((me.UltItineraryOptions != null) && (me.UltItineraryOptions.Length > 1) && (me.UltRequest.type < 3)) //OK
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltItineraryOptions")
                            {
                                if (nodeCountUltItineraryOptions == 0)
                                {
                                    nodeCountUltItineraryOptions++;
                                    nodeUIOFirstIndex = nodeCountUltItineraryOptions;
                                }
                                else
                                {
                                    nodeUIOLastIndex = nodeCountUltItineraryOptions;
                                }
                                nodeCountUIO++;
                            }
                            nodeCountUltItineraryOptions++;
                        }

                        XmlElement xmlUltItineraryOptionsElement = ObjXML.CreateElement("UltItineraryOptions", ProcessVersionNumber);
                        for (int i = 0; i < me.UltItineraryOptions.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltItineraryOptionsElement.InnerXml = xmlUltItineraryOptions;
                            }
                            else
                            {
                                xmlUltItineraryOptionsElement.InnerXml = xmlUltItineraryOptions;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltItineraryOptionsElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUIOLastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltItineraryOptionsDetail Node Create
                    int nodeCountUltItineraryOptionsDetail = 0;
                    if ((me.UltItineraryOptionsDetail != null) && (me.UltItineraryOptionsDetail.Length > 1) && (me.UltRequest.type < 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltItineraryOptionsDetail")
                            {
                                if (nodeCountUltItineraryOptionsDetail == 0)
                                {
                                    nodeCountUltItineraryOptionsDetail++;
                                    nodeUIODFirstIndex = nodeCountUltItineraryOptionsDetail;
                                }
                                else
                                {
                                    nodeUIODLastIndex = nodeCountUltItineraryOptionsDetail;
                                }
                                nodeCountUIOD++;
                            }
                            nodeCountUltItineraryOptionsDetail++;
                        }

                        XmlElement xmlUltItineraryOptionsDetailElement = ObjXML.CreateElement("UltItineraryOptionsDetail", ProcessVersionNumber);
                        for (int i = 0; i < me.UltItineraryOptionsDetail.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltItineraryOptionsDetailElement.InnerXml = xmlUltItineraryOptionsDetail;
                            }
                            else
                            {
                                xmlUltItineraryOptionsDetailElement.InnerXml = xmlUltItineraryOptionsDetail;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltItineraryOptionsDetailElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUIODLastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltPAClient Node Create
                    int nodeCountPAClient = 0;
                    if ((me.UltPAClient != null) && (me.UltPAClient.Length > 1) && (me.UltRequest.type >= 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltPAClient")
                            {
                                if (nodeCountPAClient == 0)
                                {
                                    nodeCountPAClient++;
                                    nodeUPACFirstIndex = nodeCountPAClient;
                                }
                                else
                                {
                                    nodeUPACLastIndex = nodeCountPAClient;
                                }
                                nodeCountUPAC++;
                            }
                            nodeCountPAClient++;
                        }

                        XmlElement xmlUltPAClientElement = ObjXML.CreateElement("UltPAClient", ProcessVersionNumber);
                        for (int i = 0; i < me.UltPAClient.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltPAClientElement.InnerXml = xmlUltPAClient;
                            }
                            else
                            {
                                xmlUltPAClientElement.InnerXml = xmlUltPAClient;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltPAClientElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUPACLastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltExpenseAccountDetail Node Create
                    int nodeCountUltExpenseAccountDetail = 0;
                    if ((me.UltExpenseAccountDetail != null) && (me.UltExpenseAccountDetail.Length > 1) && (me.UltRequest.type >= 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltExpenseAccountDetail")
                            {
                                if (nodeCountUltExpenseAccountDetail == 0)
                                {
                                    nodeCountUltExpenseAccountDetail++;
                                    nodeUEADFirstIndex = nodeCountUltExpenseAccountDetail;
                                }
                                else
                                {
                                    nodeUEADLastIndex = nodeCountUltExpenseAccountDetail;
                                }
                                nodeCountUEAD++;
                            }
                            nodeCountUltExpenseAccountDetail++;
                        }

                        XmlElement xmlUltExpenseAccountDetailElement = ObjXML.CreateElement("UltExpenseAccountDetail", ProcessVersionNumber);
                        for (int i = 0; i < me.UltPAClient.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltExpenseAccountDetailElement.InnerXml = xmlUltExpenseAccountDetail;
                            }
                            else
                            {
                                xmlUltExpenseAccountDetailElement.InnerXml = xmlUltExpenseAccountDetail;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltExpenseAccountDetailElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUEADLastIndex]);
                            }
                        }
                    }
                    #endregion

                    int ui = 0, uio = 0, uiod = 0, pac = 0, ead = 0;
                    foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                    {
                        #region Global Ultapprove
                        if (me.UltApprove != null)
                        {
                            if (globalNode.Name == "UltApprove")
                            {
                                globalNode.InnerXml = "<approved xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approved.ToString().ToLower() + "</approved><approverName xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverName + "</approverName><approverLogin xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverLogin + "</approverLogin><approverEmail xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverEmail + "</approverEmail>";
                            }
                        }
                        #endregion

                        #region Global UltExpenseAccount
                        if (me.UltExpenseAccount != null)
                        {
                            if (globalNode.Name == "UltExpenseAccount")
                            {
                                if (me.UltRequest.type >= 3)
                                {
                                    globalNode.InnerXml = "<idExpenseAccount xmlns=\"http://processSchema.eGastos/\">0</idExpenseAccount><idRequest xmlns=\"http://processSchema.eGastos/\">0</idRequest><nationalManagerLogin xmlns=\"http://processSchema.eGastos/\"> </nationalManagerLogin><nationalManagerName xmlns=\"http://processSchema.eGastos/\"> </nationalManagerName><debitCard xmlns=\"http://processSchema.eGastos/\">false</debitCard><isCFDI xmlns=\"http://processSchema.eGastos/\">false</isCFDI><totalMiniEvent xmlns=\"http://processSchema.eGastos/\">0</totalMiniEvent><totalMeal xmlns=\"http://processSchema.eGastos/\">0</totalMeal><totalNationalMeal xmlns=\"http://processSchema.eGastos/\">0</totalNationalMeal><overdue xmlns=\"http://processSchema.eGastos/\">false</overdue><charged xmlns=\"http://processSchema.eGastos/\">false</charged><strike xmlns=\"http://processSchema.eGastos/\">false</strike>";
                                    foreach (XmlNode UEA in globalNode)
                                    {
                                        if (UEA.Name == "charged")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.charged.ToString().ToLower();
                                        }
                                        if (UEA.Name == "debitCard")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.debitCard.ToString().ToLower();
                                        }
                                        if (UEA.Name == "idExpenseAccount")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.idExpenseAccount.ToString();
                                        }
                                        if (UEA.Name == "idRequest")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.idRequest.ToString();
                                        }
                                        if (UEA.Name == "isCFDI")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.isCFDI.ToString().ToLower();
                                        }
                                        if (UEA.Name == "nationalManagerLogin")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.nationalManagerLogin;
                                        }
                                        if (UEA.Name == "nationalManagerName")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.nationalManagerName;
                                        }
                                        if (UEA.Name == "overdue")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.overdue.ToString().ToLower();
                                        }
                                        if (UEA.Name == "strike")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.strike.ToString().ToLower();
                                        }
                                        if (UEA.Name == "totalMeal")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.totalMeal.ToString();
                                        }
                                        if (UEA.Name == "totalMiniEvent")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.totalMiniEvent.ToString();
                                        }
                                        if (UEA.Name == "totalNationalMeal")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.totalNationalMeal.ToString();
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Global UltExpenseFlowVariables
                        if (me.UltExpenseFlowVariables != null)
                        {
                            if (globalNode.Name == "UltExpenseFlowVariables")
                            {
                                globalNode.InnerXml = "<summaryText xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.summaryText + "</summaryText><jobFunctionResponsible xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionResponsible + "</jobFunctionResponsible><activeDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirAreaGastos.ToString().ToLower() + "</activeDirAreaGastos><activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirFinanzasGastos.ToString().ToLower() + "</activeDirFinanzasGastos><activeManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeManager.ToString().ToLower() + "</activeManager><jobFunctionControlling xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionControlling + "</jobFunctionControlling><jobFunctionNationalManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionNationalManager + "</jobFunctionNationalManager><jobFunctionAutorizador1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador1 + "</jobFunctionAutorizador1><jobFunctionAutorizador2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador2 + "</jobFunctionAutorizador2><jobFunctionAutorizador3 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador3 + "</jobFunctionAutorizador3><jobFunctionAutorizador4 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador4 + "</jobFunctionAutorizador4><jobFunctionController1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController1 + "</jobFunctionController1><jobFunctionController2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController2 + "</jobFunctionController2><jobFunctionObservador xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionObservador + "</jobFunctionObservador><jobFunctionDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionDirAreaGastos + "</jobFunctionDirAreaGastos><jobFunctionFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionFinanzasGastos + "</jobFunctionFinanzasGastos><activeDirGralGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirGralGastos.ToString().ToLower() + "</activeDirGralGastos>";
                            }
                        }
                        #endregion

                        #region Global UltRequest
                        if (me.UltRequest != null)
                        {
                            if (globalNode.Name == "UltRequest")
                            {
                                int cont = 0;
                                globalNode.InnerXml = "<idRequest xmlns='http://processSchema.eGastos/'></idRequest><requestDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requestDate><companyName xmlns='http://processSchema.eGastos/'></companyName><companyCode xmlns='http://processSchema.eGastos/'>0</companyCode><CeCoCode xmlns='http://processSchema.eGastos/'>0</CeCoCode><CeCoName xmlns='http://processSchema.eGastos/'></CeCoName><CeCoMiniCode xmlns='http://processSchema.eGastos/'>0</CeCoMiniCode><CeCoMiniName xmlns='http://processSchema.eGastos/'></CeCoMiniName><isMiniEvent xmlns='http://processSchema.eGastos/'>false</isMiniEvent><arrival xmlns='http://processSchema.eGastos/'></arrival><departureDate xmlns='http://processSchema.eGastos/'></departureDate><returnDate xmlns='http://processSchema.eGastos/'></returnDate><PEPElementId xmlns='http://processSchema.eGastos/'></PEPElementId><PEPElementName xmlns='http://processSchema.eGastos/'></PEPElementName><currencyId xmlns='http://processSchema.eGastos/'></currencyId><currencyName xmlns='http://processSchema.eGastos/'></currencyName><exchangeRate xmlns='http://processSchema.eGastos/'>0</exchangeRate><initiatorLogin xmlns='http://processSchema.eGastos/'></initiatorLogin><initiatorName xmlns='http://processSchema.eGastos/'></initiatorName><PAClientId xmlns='http://processSchema.eGastos/'></PAClientId><PAClientName xmlns='http://processSchema.eGastos/'></PAClientName><responsibleLogin xmlns='http://processSchema.eGastos/'></responsibleLogin><responsibleName xmlns='http://processSchema.eGastos/'></responsibleName><responsibleEmployeeNum xmlns='http://processSchema.eGastos/'></responsibleEmployeeNum><responsibleUserName xmlns='http://processSchema.eGastos/'></responsibleUserName><responsiblePayMethod xmlns='http://processSchema.eGastos/'></responsiblePayMethod><pasteur xmlns='http://processSchema.eGastos/'>false</pasteur><areaId xmlns='http://processSchema.eGastos/'>0</areaId><areaText xmlns='http://processSchema.eGastos/'></areaText><ultimusNumber xmlns='http://processSchema.eGastos/' >0</ultimusNumber><type xmlns='http://processSchema.eGastos/'>0</type><typeName xmlns='http://processSchema.eGastos/'></typeName><status xmlns='http://processSchema.eGastos/'>0</status><statusName xmlns='http://processSchema.eGastos/'></statusName><salesForce xmlns='http://processSchema.eGastos/' >false</salesForce>";
                                foreach (XmlNode UR in globalNode)
                                {
                                    cont++;
                                    if (UR.Name == "areaId") { UR.InnerText = me.UltRequest.areaId.ToString(); }
                                    if (UR.Name == "areaText") { UR.InnerText = me.UltRequest.areaText; }
                                    if (UR.Name == "arrival") { UR.InnerText = me.UltRequest.arrival; }
                                    if (UR.Name == "CeCoCode") { UR.InnerText = me.UltRequest.CeCoCode.ToString(); }
                                    if (UR.Name == "CeCoMiniCode") { UR.InnerText = me.UltRequest.CeCoMiniCode.ToString(); }
                                    if (UR.Name == "CeCoMiniName") { UR.InnerText = me.UltRequest.CeCoMiniName; }
                                    if (UR.Name == "CeCoName") { UR.InnerText = me.UltRequest.CeCoName; }
                                    if (UR.Name == "companyCode") { UR.InnerText = me.UltRequest.companyCode.ToString(); }
                                    if (UR.Name == "companyName") { UR.InnerText = me.UltRequest.companyName; }
                                    if (UR.Name == "currencyId") { UR.InnerText = me.UltRequest.currencyId; }
                                    if (UR.Name == "currencyName") { UR.InnerText = me.UltRequest.currencyName; }
                                    if (UR.Name == "departureDate") { UR.InnerText = me.UltRequest.departureDate; }
                                    if (UR.Name == "exchangeRate") { UR.InnerText = me.UltRequest.exchangeRate.ToString(); }
                                    if (UR.Name == "idRequest") { UR.InnerText = me.UltRequest.idRequest.ToString(); }
                                    if (UR.Name == "initiatorLogin") { UR.InnerText = me.UltRequest.initiatorLogin; }
                                    if (UR.Name == "initiatorName") { UR.InnerText = me.UltRequest.initiatorName; }
                                    if (UR.Name == "isMiniEvent") { UR.InnerText = me.UltRequest.isMiniEvent.ToString().ToLower(); }
                                    if (UR.Name == "PAClientId") { UR.InnerText = me.UltRequest.PAClientId; }
                                    if (UR.Name == "PAClientName") { UR.InnerText = me.UltRequest.PAClientName; }
                                    if (UR.Name == "pasteur") { UR.InnerText = me.UltRequest.pasteur.ToString().ToLower(); }
                                    if (UR.Name == "PEPElementId") { UR.InnerText = me.UltRequest.PEPElementId; }
                                    if (UR.Name == "PEPElementName") { UR.InnerText = me.UltRequest.PEPElementName; }
                                    if (UR.Name == "requestDate") { UR.InnerText = ToXMLDateFormat(me.UltRequest.requestDate); }
                                    if (UR.Name == "responsibleEmployeeNum") { UR.InnerText = me.UltRequest.responsibleEmployeeNum; }
                                    if (UR.Name == "responsibleLogin") { UR.InnerText = me.UltRequest.responsibleLogin; }
                                    if (UR.Name == "responsibleName") { UR.InnerText = me.UltRequest.responsibleName; }
                                    if (UR.Name == "responsiblePayMethod") { UR.InnerText = me.UltRequest.responsiblePayMethod; }
                                    if (UR.Name == "responsibleUserName") { UR.InnerText = me.UltRequest.responsibleUserName; }
                                    if (UR.Name == "returnDate") { UR.InnerText = me.UltRequest.returnDate; }
                                    if (UR.Name == "salesForce") { UR.InnerText = me.UltRequest.salesForce.ToString().ToLower(); }
                                    if (UR.Name == "status") { UR.InnerText = me.UltRequest.status.ToString(); }
                                    if (UR.Name == "statusName") { UR.InnerText = me.UltRequest.statusName; }
                                    if (UR.Name == "type") { UR.InnerText = me.UltRequest.type.ToString(); }
                                    if (UR.Name == "typeName") { UR.InnerText = me.UltRequest.typeName; }
                                    if (UR.Name == "ultimusNumber") { UR.InnerText = me.UltRequest.ultimusNumber.ToString(); }
                                }
                            }
                        }
                        #endregion

                        #region Global UltRequester
                        if (me.UltRequester != null)
                        {
                            if (globalNode.Name == "UltRequester")
                            {
                                globalNode.InnerXml = "<requesterName xmlns='http://processSchema.eGastos/'> </requesterName><requesterLogin xmlns='http://processSchema.eGastos/'> </requesterLogin><requesterEmail xmlns='http://processSchema.eGastos/'> </requesterEmail><requesterDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requesterDate>";
                                foreach (XmlNode UR in globalNode)
                                {
                                    if (UR.Name == "requesterDate") { UR.InnerText = ToXMLDateFormat(me.UltRequester.requesterDate); }
                                    if (UR.Name == "requesterEmail") { UR.InnerText = me.UltRequester.requesterEmail; }
                                    if (UR.Name == "requesterLogin") { UR.InnerText = me.UltRequester.requesterLogin; }
                                    if (UR.Name == "requesterName") { UR.InnerText = me.UltRequester.requesterName; }
                                }
                            }
                        }
                        #endregion

                        #region Global UltResponsible
                        if (me.UltResponsible != null)
                        {
                            if (globalNode.Name == "UltResponsible")
                            {
                                foreach (XmlNode UR in globalNode)
                                {
                                    if (UR.Name == "responsibleEmail") { UR.InnerText = me.UltResponsible.responsibleEmail; }
                                    if (UR.Name == "responsibleLogin") { UR.InnerText = me.UltResponsible.responsibleLogin; }
                                    if (UR.Name == "responsibleName") { UR.InnerText = me.UltResponsible.responsibleName; }
                                }
                            }
                        }
                        #endregion

                        #region Global UltItinerary[]
                        if ((me.UltItinerary != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.itinerary) && (me.UltItinerary.Length > 0))
                        {

                            if (globalNode.Name == "UltItinerary")
                            {
                                globalNode.InnerXml = xmlUltItinerary;

                                foreach (XmlNode UI in globalNode)
                                {
                                    if (UI.Name == "idItinerary") { UI.InnerText = me.UltItinerary[ui].idItinerary.ToString(); }
                                    if (UI.Name == "idMissionOrder") { UI.InnerText = me.UltItinerary[ui].idMissionOrder.ToString(); }
                                    if (UI.Name == "idConsecutive") { UI.InnerText = me.UltItinerary[ui].idConsecutive.ToString(); }
                                    if (UI.Name == "idLedgerAccount") { UI.InnerText = me.UltItinerary[ui].idLedgerAccount.ToString(); }
                                    if (UI.Name == "nameLedgerAccount") { UI.InnerText = me.UltItinerary[ui].nameLedgerAccount; }
                                    if (UI.Name == "departureHour") { UI.InnerText = me.UltItinerary[ui].departureHour; }
                                    if (UI.Name == "returnHour") { UI.InnerText = me.UltItinerary[ui].returnHour; }
                                    if (UI.Name == "observations") { UI.InnerText = me.UltItinerary[ui].observations; }
                                    if (UI.Name == "travelType") { UI.InnerText = me.UltItinerary[ui].travelType.ToString(); }
                                    if (UI.Name == "nameTravelType") { UI.InnerText = me.UltItinerary[ui].nameTravelType; }
                                    if (UI.Name == "departureCountry") { UI.InnerText = me.UltItinerary[ui].departureCountry; }
                                    if (UI.Name == "departureCity") { UI.InnerText = me.UltItinerary[ui].departureCity; }
                                    if (UI.Name == "arrivalCountry") { UI.InnerText = me.UltItinerary[ui].arrivalCountry; }
                                    if (UI.Name == "arrivalCity") { UI.InnerText = me.UltItinerary[ui].arrivalCity; }
                                    if (UI.Name == "departureDate") { UI.InnerText = ToXMLDateFormat(me.UltItinerary[ui].departureDate); }
                                    if (UI.Name == "arrivalDate") { UI.InnerText = ToXMLDateFormat(me.UltItinerary[ui].arrivalDate); }
                                    if (UI.Name == "status") { UI.InnerText = me.UltItinerary[ui].status.ToString().ToLower(); }
                                }
                                ui++;
                            }
                        }
                        #endregion

                        #region Global UltItineraryOptions[]
                        if ((me.UltItineraryOptions != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.itinerary) && (me.UltItineraryOptions.Length > 0))
                        {
                            if (globalNode.Name == "UltItineraryOptions")
                            {
                                globalNode.InnerXml = xmlUltItineraryOptions;

                                foreach (XmlNode UIO in globalNode)
                                {
                                    if (UIO.Name == "idItineraryOption") { UIO.InnerText = me.UltItineraryOptions[uio].idItineraryOption.ToString(); }
                                    if (UIO.Name == "idMissionOrder") { UIO.InnerText = me.UltItineraryOptions[uio].idMissionOrder.ToString(); }
                                    if (UIO.Name == "idRate") { UIO.InnerText = me.UltItineraryOptions[uio].idRate.ToString(); }
                                    if (UIO.Name == "quoteRate") { UIO.InnerText = me.UltItineraryOptions[uio].quoteRate.ToString(); }
                                    if (UIO.Name == "observations") { UIO.InnerText = me.UltItineraryOptions[uio].observations; }
                                    if (UIO.Name == "confirmed") { UIO.InnerText = me.UltItineraryOptions[uio].confirmed.ToString().ToLower(); }
                                    if (UIO.Name == "lastDayPurchase") { UIO.InnerText = ToXMLDateFormat(me.UltItineraryOptions[uio].lastDayPurchase); }
                                }
                                uio++;
                            }
                        }
                        #endregion

                        #region Global UltItineraryOptionsDetail[]
                        if ((me.UltItineraryOptionsDetail != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.itinerary) && (me.UltItineraryOptionsDetail.Length > 0))
                        {
                            if (globalNode.Name == "UltItineraryOptionsDetail")
                            {
                                globalNode.InnerXml = xmlUltItineraryOptionsDetail;

                                foreach (XmlNode UIOD in globalNode)
                                {
                                    if (UIOD.Name == "airlineFlight") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].airlineFlight; }
                                    if (UIOD.Name == "arrival") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].arrival; }
                                    if (UIOD.Name == "arrivalDate") { UIOD.InnerText = ToXMLDateFormat(me.UltItineraryOptionsDetail[uiod].arrivalDate); }
                                    if (UIOD.Name == "departure") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].departure; }
                                    if (UIOD.Name == "departureDate") { UIOD.InnerText = ToXMLDateFormat(me.UltItineraryOptionsDetail[uiod].departureDate); }
                                    if (UIOD.Name == "idItineraryOption") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].idItineraryOption.ToString(); }
                                    if (UIOD.Name == "idItineraryOptionsDetail") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].idItineraryOptionsDetail.ToString(); }
                                    if (UIOD.Name == "idMissionOrder") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].idMissionOrder.ToString(); }
                                    if (UIOD.Name == "lapseTime") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].lapseTime.ToString(); }
                                }
                                uiod++;
                            }
                        }
                        #endregion

                        #region Global UltPAClient[]
                        if ((me.UltPAClient != null) && (me.UltRequest.type >= 3) && (me.UltPAClient.Length > 0))
                        {
                            if (globalNode.Name == "UltPAClient")
                            {
                                globalNode.InnerXml = xmlUltPAClient;

                                foreach (XmlNode UPAC in globalNode)
                                {
                                    if (UPAC.Name == "code") { UPAC.InnerText = me.UltPAClient[pac].code; }
                                    if (UPAC.Name == "idExpenseAccountDetail") { UPAC.InnerText = me.UltPAClient[pac].idExpenseAccountDetail.ToString(); }
                                    if (UPAC.Name == "name") { UPAC.InnerText = me.UltPAClient[pac].name; }
                                }
                                pac++;
                            }
                        }
                        #endregion

                        #region Global UltExpenseAccountDetail[]
                        if ((me.UltExpenseAccountDetail != null) && (me.UltRequest.type >= 3) && (me.UltExpenseAccountDetail.Length > 0))
                        {
                            if (globalNode.Name == "UltExpenseAccountDetail")
                            {
                                globalNode.InnerXml = xmlUltExpenseAccountDetail;

                                foreach (XmlNode UEAD in globalNode)
                                {
                                    if (UEAD.Name == "idExpenseAccountDetail") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idExpenseAccountDetail.ToString(); }
                                    if (UEAD.Name == "idExpenseAccount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idExpenseAccount.ToString(); }
                                    if (UEAD.Name == "expenseDate") { UEAD.InnerText = ToXMLDateFormat(me.UltExpenseAccountDetail[ead].expenseDate); }
                                    if (UEAD.Name == "idAccount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idAccount.ToString(); }
                                    if (UEAD.Name == "accountName") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].accountName; }
                                    if (UEAD.Name == "amount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].amount.ToString(); }
                                    if (UEAD.Name == "invoiceNumber") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].invoiceNumber; }
                                    if (UEAD.Name == "place") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].place; }
                                    if (UEAD.Name == "numberOfDiners") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].numberOfDiners.ToString(); }
                                    if (UEAD.Name == "IVA") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].IVA.ToString(); }
                                    if (UEAD.Name == "healthProfessional") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].healthProfessional.ToString().ToLower(); }
                                    if (UEAD.Name == "discount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].discount.ToString(); }
                                    if (UEAD.Name == "hasPAClient") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].hasPAClient.ToString().ToLower(); }
                                    if (UEAD.Name == "IVATypeId") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].IVATypeId; }
                                    if (UEAD.Name == "IVATypeName") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].IVATypeName; }
                                    if (UEAD.Name == "total") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].total.ToString(); }
                                    if (UEAD.Name == "observationId") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].observationId.ToString(); }
                                    if (UEAD.Name == "observationName") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].observationName; }
                                    if (UEAD.Name == "idXml") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idXml.ToString(); }
                                    if (UEAD.Name == "amountCFDI") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].amountCFDI.ToString(); }
                                    if (UEAD.Name == "ivaCFDI") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].ivaCFDI.ToString(); }
                                    if (UEAD.Name == "idExtract") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idExtract.ToString(); }
                                    if (UEAD.Name == "amountExtract") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].amountExtract.ToString(); }
                                    if (UEAD.Name == "conciliated") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].conciliated.ToString().ToLower(); }
                                    if (UEAD.Name == "strike") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].strike.ToString().ToLower(); }
                                    if (UEAD.Name == "status") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].status.ToString().ToLower(); }
                                }
                                ead++;
                            }
                        }
                        #endregion

                    }

                    #region StepSchemaUltHotel
                    if ((me.UltHotel != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.hotel))
                    {
                        int i = me.UltHotel.Length - 1;
                        foreach (object obj in me.UltHotel)
                        {
                            XmlNode xmlUltHotelElement = ObjXML.CreateNode("element", "StepSchemaUltHotel", "");

                            xmlUltHotelElement.InnerXml = xmlStepSchemaUltHotel;
                            ObjXML.ChildNodes[1].InsertBefore(xmlUltHotelElement, ObjXML.ChildNodes[1].ChildNodes[2]);

                            foreach (XmlNode UH in xmlUltHotelElement)
                            {
                                if (UH.Name == "idHotel") { UH.InnerText = UH.InnerText = me.UltHotel[i].idHotel.ToString(); }
                                if (UH.Name == "idMissionOrder") { UH.InnerText = me.UltHotel[i].idMissionOrder.ToString(); }
                                if (UH.Name == "idRated") { UH.InnerText = me.UltHotel[i].idRated.ToString(); }
                                if (UH.Name == "idConsecutive") { UH.InnerText = me.UltHotel[i].idConsecutive.ToString(); }
                                if (UH.Name == "idLegerAccount") { UH.InnerText = me.UltHotel[i].idLegerAccount.ToString(); }
                                if (UH.Name == "checkInDate") { UH.InnerText = ToXMLDateFormat(me.UltHotel[i].checkInDate); }
                                if (UH.Name == "checkoutDate") { UH.InnerText = ToXMLDateFormat(me.UltHotel[i].checkoutDate); }
                                if (UH.Name == "quotedRate") { UH.InnerText = me.UltHotel[i].quotedRate.ToString(); }
                                if (UH.Name == "realRate") { UH.InnerText = me.UltHotel[i].realRate.ToString(); }
                                if (UH.Name == "IVA") { UH.InnerText = me.UltHotel[i].IVA.ToString(); }
                                if (UH.Name == "hotelTax") { UH.InnerText = me.UltHotel[i].hotelTax.ToString(); }
                                if (UH.Name == "otherTaxes") { UH.InnerText = me.UltHotel[i].otherTaxes.ToString(); }
                                if (UH.Name == "status") { UH.InnerText = me.UltHotel[i].status.ToString().ToLower(); }
                                if (UH.Name == "lineStatus") { UH.InnerText = me.UltHotel[i].lineStatus.ToString(); }
                            }
                            i--;
                        }
                    }
                    #endregion

                    XmlDataDocument ObjXMLSend = new XmlDataDocument();
                    ObjXMLSend.InnerXml = ObjXML.InnerXml.Replace("StepSchemaUltHotel xmlns=\"\"", "StepSchemaUltHotel");

                    bolResultado = ult_obj.LaunchIncident(fd.UserLogin, summary, "Incidente criado por: ", false, 9, ObjXMLSend.InnerXml, true, out intIncident, out strError);
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

            WSeGastosPharma.eGastos_Pharma_BC ult_obj = new WSeGastosPharma.eGastos_Pharma_BC();
            WSeGastosPharma.SchemaFile[] schemas;


            // REQUEST  DATA (rd)           
            string strHoraAtual = ToXMLDateFormat(DateTime.Now);
            string xmlStepSchemaUltHotel = "<idHotel xmlns='http://processSchema.eGastos/'>0</idHotel><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idRated xmlns='http://processSchema.eGastos/'>0</idRated><idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive><idLegerAccount xmlns='http://processSchema.eGastos/'>0</idLegerAccount><nameLegerAccount xmlns='http://processSchema.eGastos/'>asd</nameLegerAccount><country xmlns='http://processSchema.eGastos/'>asd</country><city xmlns='http://processSchema.eGastos/'>asd</city><observations xmlns='http://processSchema.eGastos/'>asd</observations><checkInDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkInDate><checkoutDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkoutDate><hotelName xmlns='http://processSchema.eGastos/'>asd</hotelName><reservation xmlns='http://processSchema.eGastos/'>asd</reservation><telephone xmlns='http://processSchema.eGastos/'>asd</telephone><address xmlns='http://processSchema.eGastos/'>asd</address><quotedRate xmlns='http://processSchema.eGastos/'>0</quotedRate><realRate xmlns='http://processSchema.eGastos/'>0</realRate><IVA xmlns='http://processSchema.eGastos/'>0</IVA><hotelTax xmlns='http://processSchema.eGastos/'>0</hotelTax><otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes><status xmlns='http://processSchema.eGastos/'>true</status><lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus><lineStatusName xmlns='http://processSchema.eGastos/'>asd</lineStatusName>";
            string xmlStepSchemaUltApprovalHistory = "<stepName xmlns='http://processSchema.eGastos/'></stepName><approverName xmlns='http://processSchema.eGastos/'></approverName><approverLogin xmlns='http://processSchema.eGastos/'></approverLogin><userEmail xmlns='http://processSchema.eGastos/'></userEmail><approveDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</approveDate><comments xmlns='http://processSchema.eGastos/'></comments><approveStatus xmlns='http://processSchema.eGastos/'></approveStatus>";
            string xmlStepSchemaUltMissionOrder = "<idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idRequest xmlns='http://processSchema.eGastos/'>0</idRequest><idAgencyResponse xmlns='http://processSchema.eGastos/'>0</idAgencyResponse><statusAgencyProcess xmlns='http://processSchema.eGastos/'>0</statusAgencyProcess><statusAgencySend xmlns='http://processSchema.eGastos/'>0</statusAgencySend><countAgencyWait xmlns='http://processSchema.eGastos/'>0</countAgencyWait><idAgencyLog xmlns='http://processSchema.eGastos/'>0</idAgencyLog><travelId xmlns='http://processSchema.eGastos/'></travelId><travelName xmlns='http://processSchema.eGastos/'></travelName><objective xmlns='http://processSchema.eGastos/'></objective><advance xmlns='http://processSchema.eGastos/'>0</advance><nationalCurrency xmlns='http://processSchema.eGastos/'>0</nationalCurrency><advanceApply xmlns='http://processSchema.eGastos/'>false</advanceApply><itinerary xmlns='http://processSchema.eGastos/'>false</itinerary><hotel xmlns='http://processSchema.eGastos/'>false</hotel><comment xmlns='http://processSchema.eGastos/'></comment><exceededAdvance xmlns='http://processSchema.eGastos/'>false</exceededAdvance><missionOrderType xmlns='http://processSchema.eGastos/'>0</missionOrderType><missionOrderTypeText xmlns='http://processSchema.eGastos/'>0</missionOrderTypeText><advanceAndDebitCard xmlns='http://processSchema.eGastos/'>false</advanceAndDebitCard>";
            string xmlStepSchemaUltGetThere = "<idGetThere xmlns='http://processSchema.eGastos/'>0</idGetThere><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><conceptId xmlns='http://processSchema.eGastos/'>0</conceptId><conceptText xmlns='http://processSchema.eGastos/'> </conceptText><lowCost xmlns='http://processSchema.eGastos/'>false</lowCost><justification xmlns='http://processSchema.eGastos/'> </justification><cheapestRate xmlns='http://processSchema.eGastos/'> </cheapestRate><outPolitic xmlns='http://processSchema.eGastos/'>false</outPolitic><outPoliticMessage xmlns='http://processSchema.eGastos/'> </outPoliticMessage>";
            string xmlUltItinerary = "<idItinerary xmlns='http://processSchema.eGastos/'>0</idItinerary><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive><idLedgerAccount xmlns='http://processSchema.eGastos/'>0</idLedgerAccount><nameLedgerAccount  xmlns='http://processSchema.eGastos/'> </nameLedgerAccount><departureHour  xmlns='http://processSchema.eGastos/'>00:00</departureHour><returnHour  xmlns='http://processSchema.eGastos/'>00:00</returnHour><observations  xmlns='http://processSchema.eGastos/'> </observations><travelType xmlns='http://processSchema.eGastos/'>0</travelType><nameTravelType  xmlns='http://processSchema.eGastos/'> </nameTravelType><departureCountry  xmlns='http://processSchema.eGastos/'> </departureCountry><departureCity  xmlns='http://processSchema.eGastos/'> </departureCity><arrivalCountry  xmlns='http://processSchema.eGastos/'> </arrivalCountry><arrivalCity  xmlns='http://processSchema.eGastos/'> </arrivalCity><departureDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</departureDate><arrivalDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</arrivalDate><status xmlns='http://processSchema.eGastos/'>false</status>";
            string xmlUltItineraryOptions = "<idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><idRate xmlns='http://processSchema.eGastos/'>0</idRate><quoteRate xmlns='http://processSchema.eGastos/'>0</quoteRate><observations xmlns='http://processSchema.eGastos/'> </observations><confirmed xmlns='http://processSchema.eGastos/'>false</confirmed><lastDayPurchase xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</lastDayPurchase>";
            string xmlUltItineraryOptionsDetail = "<idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>0</idItineraryOptionsDetail><idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption><idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder><airlineFlight xmlns='http://processSchema.eGastos/'> </airlineFlight><departure xmlns='http://processSchema.eGastos/'> </departure><arrival xmlns='http://processSchema.eGastos/'> </arrival><departureDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</departureDate><arrivalDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</arrivalDate><lapseTime xmlns='http://processSchema.eGastos/'>0</lapseTime>";
            string xmlUltPAClient = "<idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail><code xmlns='http://processSchema.eGastos/'> </code><name xmlns='http://processSchema.eGastos/'> </name>";
            string xmlUltExpenseAccountDetail = "<idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail><idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount><expenseDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</expenseDate><idAccount xmlns='http://processSchema.eGastos/'>0</idAccount><accountName xmlns='http://processSchema.eGastos/'> </accountName><amount xmlns='http://processSchema.eGastos/'>0</amount><invoiceNumber xmlns='http://processSchema.eGastos/'> </invoiceNumber><place xmlns='http://processSchema.eGastos/'> </place><numberOfDiners xmlns='http://processSchema.eGastos/'>0</numberOfDiners><IVA xmlns='http://processSchema.eGastos/'>0</IVA><healthProfessional xmlns='http://processSchema.eGastos/'>false</healthProfessional><discount xmlns='http://processSchema.eGastos/'>0</discount><hasPAClient xmlns='http://processSchema.eGastos/'>false</hasPAClient><IVATypeId xmlns='http://processSchema.eGastos/'> </IVATypeId><IVATypeName xmlns='http://processSchema.eGastos/'> </IVATypeName><total xmlns='http://processSchema.eGastos/'>0</total><observationId xmlns='http://processSchema.eGastos/'>0</observationId><observationName xmlns='http://processSchema.eGastos/'> </observationName><idXml xmlns='http://processSchema.eGastos/'>0</idXml><amountCFDI xmlns='http://processSchema.eGastos/'>0</amountCFDI><ivaCFDI xmlns='http://processSchema.eGastos/'>0</ivaCFDI><idExtract xmlns='http://processSchema.eGastos/'>0</idExtract><amountExtract xmlns='http://processSchema.eGastos/'>0</amountExtract><conciliated xmlns='http://processSchema.eGastos/'>false</conciliated><strike xmlns='http://processSchema.eGastos/'>false</strike><status xmlns='http://processSchema.eGastos/'>false</status>";

            int nodeCountUI = 0, nodeUILastIndex = 0, nodeUIFirstIndex = 0;
            int nodeCountUIO = 0, nodeUIOLastIndex = 0, nodeUIOFirstIndex = 0;
            int nodeCountUIOD = 0, nodeUIODLastIndex = 0, nodeUIODFirstIndex = 0;
            int nodeCountUPAC = 0, nodeUPACLastIndex = 0, nodeUPACFirstIndex = 0;
            int nodeCountUEAD = 0, nodeUEADLastIndex = 0, nodeUEADFirstIndex = 0;

            string summary = me.UltExpenseFlowVariables.summaryText;
            string strError = "";
            string strxml;

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

                    #region StepSchemaUltApprovalHistory
                    ObjXML.ChildNodes[1].ChildNodes[1].InnerXml = xmlStepSchemaUltApprovalHistory;
                    foreach (XmlNode SSUAH in ObjXML.ChildNodes[1].ChildNodes[1])
                    {
                        if (SSUAH.Name == "approveDate") { SSUAH.InnerText = ToXMLDateFormat(me.UltApprovalHistory[0].approveDate); }
                        if (SSUAH.Name == "approverLogin") { SSUAH.InnerText = me.UltApprovalHistory[0].approverLogin; }
                        if (SSUAH.Name == "approverName") { SSUAH.InnerText = me.UltApprovalHistory[0].approverName; }
                        if (SSUAH.Name == "approveStatus") { SSUAH.InnerText = me.UltApprovalHistory[0].approveStatus; }
                        if (SSUAH.Name == "comments") { SSUAH.InnerText = me.UltApprovalHistory[0].comments; }
                        if (SSUAH.Name == "stepName") { SSUAH.InnerText = me.UltApprovalHistory[0].stepName; }
                        if (SSUAH.Name == "userEmail") { SSUAH.InnerText = me.UltApprovalHistory[0].userEmail; }
                    };
                    #endregion

                    #region StepSchemaUltMissionOrder
                    if ((me.UltMissionOrder != null) && (me.UltRequest.type < 3))
                    {
                        ObjXML.ChildNodes[1].ChildNodes[2].InnerXml = xmlStepSchemaUltMissionOrder;
                        foreach (XmlNode SSUMO in ObjXML.ChildNodes[1].ChildNodes[2])
                        {
                            if (SSUMO.Name == "advance") { SSUMO.InnerText = me.UltMissionOrder.advance.ToString(); }
                            if (SSUMO.Name == "advanceAndDebitCard") { SSUMO.InnerText = me.UltMissionOrder.advanceAndDebitCard.ToString().ToLower(); }
                            if (SSUMO.Name == "advanceApply") { SSUMO.InnerText = me.UltMissionOrder.advanceApply.ToString().ToLower(); }
                            if (SSUMO.Name == "comment") { SSUMO.InnerText = me.UltMissionOrder.comment; }
                            if (SSUMO.Name == "countAgencyWait") { SSUMO.InnerText = me.UltMissionOrder.countAgencyWait.ToString(); }
                            if (SSUMO.Name == "exceededAdvance") { SSUMO.InnerText = me.UltMissionOrder.exceededAdvance.ToString().ToLower(); }
                            if (SSUMO.Name == "hotel") { SSUMO.InnerText = me.UltMissionOrder.hotel.ToString().ToLower(); }
                            if (SSUMO.Name == "idAgencyLog") { SSUMO.InnerText = me.UltMissionOrder.idAgencyLog.ToString(); }
                            if (SSUMO.Name == "idAgencyResponse") { SSUMO.InnerText = me.UltMissionOrder.idAgencyResponse.ToString(); }
                            if (SSUMO.Name == "idMissionOrder") { SSUMO.InnerText = me.UltMissionOrder.idMissionOrder.ToString(); }
                            if (SSUMO.Name == "idRequest") { SSUMO.InnerText = me.UltMissionOrder.idRequest.ToString(); }
                            if (SSUMO.Name == "itinerary") { SSUMO.InnerText = me.UltMissionOrder.itinerary.ToString().ToLower(); }
                            if (SSUMO.Name == "missionOrderType") { SSUMO.InnerText = me.UltMissionOrder.missionOrderType.ToString(); }
                            if (SSUMO.Name == "missionOrderTypeText") { SSUMO.InnerText = me.UltMissionOrder.missionOrderTypeText; }
                            if (SSUMO.Name == "nationalCurrency") { SSUMO.InnerText = me.UltMissionOrder.nationalCurrency.ToString(); }
                            if (SSUMO.Name == "objective") { SSUMO.InnerText = me.UltMissionOrder.objective; }
                            if (SSUMO.Name == "statusAgencyProcess") { SSUMO.InnerText = me.UltMissionOrder.statusAgencyProcess.ToString(); }
                            if (SSUMO.Name == "statusAgencySend") { SSUMO.InnerText = me.UltMissionOrder.statusAgencySend.ToString(); }
                            if (SSUMO.Name == "travelId") { SSUMO.InnerText = me.UltMissionOrder.travelId; }
                            if (SSUMO.Name == "travelName") { SSUMO.InnerText = me.UltMissionOrder.travelName; }
                        }
                    }
                    #endregion

                    #region StepSchemaUltGetThere
                    if ((me.UltGetThere != null))
                    {
                        ObjXML.ChildNodes[1].ChildNodes[3].InnerXml = xmlStepSchemaUltGetThere;
                        foreach (XmlNode SSUGT in ObjXML.ChildNodes[1].ChildNodes[3])
                        {
                            if (SSUGT.Name == "idGetThere") { SSUGT.InnerText = me.UltGetThere.idGetThere.ToString(); }
                            if (SSUGT.Name == "idMissionOrder") { SSUGT.InnerText = me.UltGetThere.idMissionOrder.ToString(); }
                            if (SSUGT.Name == "conceptId") { SSUGT.InnerText = me.UltGetThere.conceptId.ToString(); }
                            if (SSUGT.Name == "conceptText") { SSUGT.InnerText = me.UltGetThere.conceptText; }
                            if (SSUGT.Name == "lowCost") { SSUGT.InnerText = me.UltGetThere.lowCost.ToString().ToLower(); }
                            if (SSUGT.Name == "justification") { SSUGT.InnerText = me.UltGetThere.justification; }
                            if (SSUGT.Name == "cheapestRate") { SSUGT.InnerText = me.UltGetThere.cheapestRate; }
                            if (SSUGT.Name == "outPolitic") { SSUGT.InnerText = me.UltGetThere.outPolitic.ToString().ToLower(); }
                            if (SSUGT.Name == "outPoliticMessage") { SSUGT.InnerText = me.UltGetThere.outPoliticMessage; }                            
                        }
                    }
                    #endregion

                    #region UltItinerary Node Create
                    int nodeCountItinerary = 0;
                    if ((me.UltItinerary != null) && (me.UltItinerary.Length > 1) && (me.UltRequest.type < 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltItinerary")
                            {
                                if (nodeCountItinerary == 0)
                                {
                                    nodeCountItinerary++;
                                    nodeUIFirstIndex = nodeCountItinerary;
                                }
                                else
                                {
                                    nodeUILastIndex = nodeCountItinerary;
                                }
                                nodeCountUI++;
                            }
                            nodeCountItinerary++;
                        }

                        XmlElement xmlUltItineraryElement = ObjXML.CreateElement("UltItinerary", ProcessVersionNumber);
                        for (int i = 0; i < me.UltItinerary.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltItineraryElement.InnerXml = xmlUltItinerary;
                            }
                            else
                            {
                                xmlUltItineraryElement.InnerXml = xmlUltItinerary;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltItineraryElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUILastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltItineraryOptions Node Create
                    int nodeCountUltItineraryOptions = 0;
                    if ((me.UltItineraryOptions != null) && (me.UltItineraryOptions.Length > 1) && (me.UltRequest.type < 3)) //OK
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltItineraryOptions")
                            {
                                if (nodeCountUltItineraryOptions == 0)
                                {
                                    nodeCountUltItineraryOptions++;
                                    nodeUIOFirstIndex = nodeCountUltItineraryOptions;
                                }
                                else
                                {
                                    nodeUIOLastIndex = nodeCountUltItineraryOptions;
                                }
                                nodeCountUIO++;
                            }
                            nodeCountUltItineraryOptions++;
                        }

                        XmlElement xmlUltItineraryOptionsElement = ObjXML.CreateElement("UltItineraryOptions", ProcessVersionNumber);
                        for (int i = 0; i < me.UltItineraryOptions.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltItineraryOptionsElement.InnerXml = xmlUltItineraryOptions;
                            }
                            else
                            {
                                xmlUltItineraryOptionsElement.InnerXml = xmlUltItineraryOptions;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltItineraryOptionsElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUIOLastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltItineraryOptionsDetail Node Create
                    int nodeCountUltItineraryOptionsDetail = 0;
                    if ((me.UltItineraryOptionsDetail != null) && (me.UltItineraryOptionsDetail.Length > 1) && (me.UltRequest.type < 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltItineraryOptionsDetail")
                            {
                                if (nodeCountUltItineraryOptionsDetail == 0)
                                {
                                    nodeCountUltItineraryOptionsDetail++;
                                    nodeUIODFirstIndex = nodeCountUltItineraryOptionsDetail;
                                }
                                else
                                {
                                    nodeUIODLastIndex = nodeCountUltItineraryOptionsDetail;
                                }
                                nodeCountUIOD++;
                            }
                            nodeCountUltItineraryOptionsDetail++;
                        }

                        XmlElement xmlUltItineraryOptionsDetailElement = ObjXML.CreateElement("UltItineraryOptionsDetail", ProcessVersionNumber);
                        for (int i = 0; i < me.UltItineraryOptionsDetail.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltItineraryOptionsDetailElement.InnerXml = xmlUltItineraryOptionsDetail;
                            }
                            else
                            {
                                xmlUltItineraryOptionsDetailElement.InnerXml = xmlUltItineraryOptionsDetail;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltItineraryOptionsDetailElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUIODLastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltPAClient Node Create
                    int nodeCountPAClient = 0;
                    if ((me.UltPAClient != null) && (me.UltPAClient.Length > 1) && (me.UltRequest.type >= 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltPAClient")
                            {
                                if (nodeCountPAClient == 0)
                                {
                                    nodeCountPAClient++;
                                    nodeUPACFirstIndex = nodeCountPAClient;
                                }
                                else
                                {
                                    nodeUPACLastIndex = nodeCountPAClient;
                                }
                                nodeCountUPAC++;
                            }
                            nodeCountPAClient++;
                        }

                        XmlElement xmlUltPAClientElement = ObjXML.CreateElement("UltPAClient", ProcessVersionNumber);
                        for (int i = 0; i < me.UltPAClient.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltPAClientElement.InnerXml = xmlUltPAClient;
                            }
                            else
                            {
                                xmlUltPAClientElement.InnerXml = xmlUltPAClient;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltPAClientElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUPACLastIndex]);
                            }
                        }
                    }
                    #endregion

                    #region UltExpenseAccountDetail Node Create
                    int nodeCountUltExpenseAccountDetail = 0;
                    if ((me.UltExpenseAccountDetail != null) && (me.UltExpenseAccountDetail.Length > 1) && (me.UltRequest.type >= 3))
                    {
                        foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                        {
                            if (globalNode.Name == "UltExpenseAccountDetail")
                            {
                                if (nodeCountUltExpenseAccountDetail == 0)
                                {
                                    nodeCountUltExpenseAccountDetail++;
                                    nodeUEADFirstIndex = nodeCountUltExpenseAccountDetail;
                                }
                                else
                                {
                                    nodeUEADLastIndex = nodeCountUltExpenseAccountDetail;
                                }
                                nodeCountUEAD++;
                            }
                            nodeCountUltExpenseAccountDetail++;
                        }

                        XmlElement xmlUltExpenseAccountDetailElement = ObjXML.CreateElement("UltExpenseAccountDetail", ProcessVersionNumber);
                        for (int i = 0; i < me.UltPAClient.Length; i++)
                        {
                            if (i == 0)
                            {
                                xmlUltExpenseAccountDetailElement.InnerXml = xmlUltExpenseAccountDetail;
                            }
                            else
                            {
                                xmlUltExpenseAccountDetailElement.InnerXml = xmlUltExpenseAccountDetail;
                                ObjXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltExpenseAccountDetailElement, ObjXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeUEADLastIndex]);
                            }
                        }
                    }
                    #endregion

                    int ui = 0, uio = 0, uiod = 0, pac = 0, ead = 0;
                    foreach (XmlNode globalNode in ObjXML.ChildNodes[1].ChildNodes[0])
                    {
                        #region Global Ultapprove
                        if (me.UltApprove != null)
                        {
                            if (globalNode.Name == "UltApprove")
                            {
                                globalNode.InnerXml = "<approved xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approved.ToString().ToLower() + "</approved><approverName xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverName + "</approverName><approverLogin xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverLogin + "</approverLogin><approverEmail xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverEmail + "</approverEmail>";
                            }
                        }
                        #endregion

                        #region Global UltExpenseAccount
                        if (me.UltExpenseAccount != null)
                        {
                            if (globalNode.Name == "UltExpenseAccount")
                            {
                                if (me.UltRequest.type >= 3)
                                {
                                    globalNode.InnerXml = "<idExpenseAccount xmlns=\"http://processSchema.eGastos/\">0</idExpenseAccount><idRequest xmlns=\"http://processSchema.eGastos/\">0</idRequest><nationalManagerLogin xmlns=\"http://processSchema.eGastos/\"> </nationalManagerLogin><nationalManagerName xmlns=\"http://processSchema.eGastos/\"> </nationalManagerName><debitCard xmlns=\"http://processSchema.eGastos/\">false</debitCard><isCFDI xmlns=\"http://processSchema.eGastos/\">false</isCFDI><totalMiniEvent xmlns=\"http://processSchema.eGastos/\">0</totalMiniEvent><totalMeal xmlns=\"http://processSchema.eGastos/\">0</totalMeal><totalNationalMeal xmlns=\"http://processSchema.eGastos/\">0</totalNationalMeal><overdue xmlns=\"http://processSchema.eGastos/\">false</overdue><charged xmlns=\"http://processSchema.eGastos/\">false</charged><strike xmlns=\"http://processSchema.eGastos/\">false</strike>";
                                    foreach (XmlNode UEA in globalNode)
                                    {
                                        if (UEA.Name == "charged")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.charged.ToString().ToLower();
                                        }
                                        if (UEA.Name == "debitCard")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.debitCard.ToString().ToLower();
                                        }
                                        if (UEA.Name == "idExpenseAccount")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.idExpenseAccount.ToString();
                                        }
                                        if (UEA.Name == "idRequest")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.idRequest.ToString();
                                        }
                                        if (UEA.Name == "isCFDI")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.isCFDI.ToString().ToLower();
                                        }
                                        if (UEA.Name == "nationalManagerLogin")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.nationalManagerLogin;
                                        }
                                        if (UEA.Name == "nationalManagerName")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.nationalManagerName;
                                        }
                                        if (UEA.Name == "overdue")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.overdue.ToString().ToLower();
                                        }
                                        if (UEA.Name == "strike")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.strike.ToString().ToLower();
                                        }
                                        if (UEA.Name == "totalMeal")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.totalMeal.ToString();
                                        }
                                        if (UEA.Name == "totalMiniEvent")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.totalMiniEvent.ToString();
                                        }
                                        if (UEA.Name == "totalNationalMeal")
                                        {
                                            UEA.InnerText = me.UltExpenseAccount.totalNationalMeal.ToString();
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Global UltExpenseFlowVariables
                        if (me.UltExpenseFlowVariables != null)
                        {
                            if (globalNode.Name == "UltExpenseFlowVariables")
                            {
                                globalNode.InnerXml = "<summaryText xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.summaryText + "</summaryText><jobFunctionResponsible xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionResponsible + "</jobFunctionResponsible><activeDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirAreaGastos.ToString().ToLower() + "</activeDirAreaGastos><activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirFinanzasGastos.ToString().ToLower() + "</activeDirFinanzasGastos><activeManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeManager.ToString().ToLower() + "</activeManager><jobFunctionControlling xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionControlling + "</jobFunctionControlling><jobFunctionNationalManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionNationalManager + "</jobFunctionNationalManager><jobFunctionAutorizador1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador1 + "</jobFunctionAutorizador1><jobFunctionAutorizador2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador2 + "</jobFunctionAutorizador2><jobFunctionAutorizador3 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador3 + "</jobFunctionAutorizador3><jobFunctionAutorizador4 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador4 + "</jobFunctionAutorizador4><jobFunctionController1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController1 + "</jobFunctionController1><jobFunctionController2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController2 + "</jobFunctionController2><jobFunctionObservador xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionObservador + "</jobFunctionObservador><jobFunctionDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionDirAreaGastos + "</jobFunctionDirAreaGastos><jobFunctionFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionFinanzasGastos + "</jobFunctionFinanzasGastos><activeDirGralGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirGralGastos.ToString().ToLower() + "</activeDirGralGastos>";
                            }
                        }
                        #endregion

                        #region Global UltRequest
                        if (me.UltRequest != null)
                        {
                            if (globalNode.Name == "UltRequest")
                            {
                                int cont = 0;
                                globalNode.InnerXml = "<idRequest xmlns='http://processSchema.eGastos/'></idRequest><requestDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requestDate><companyName xmlns='http://processSchema.eGastos/'></companyName><companyCode xmlns='http://processSchema.eGastos/'>0</companyCode><CeCoCode xmlns='http://processSchema.eGastos/'>0</CeCoCode><CeCoName xmlns='http://processSchema.eGastos/'></CeCoName><CeCoMiniCode xmlns='http://processSchema.eGastos/'>0</CeCoMiniCode><CeCoMiniName xmlns='http://processSchema.eGastos/'></CeCoMiniName><isMiniEvent xmlns='http://processSchema.eGastos/'>false</isMiniEvent><arrival xmlns='http://processSchema.eGastos/'></arrival><departureDate xmlns='http://processSchema.eGastos/'></departureDate><returnDate xmlns='http://processSchema.eGastos/'></returnDate><PEPElementId xmlns='http://processSchema.eGastos/'></PEPElementId><PEPElementName xmlns='http://processSchema.eGastos/'></PEPElementName><currencyId xmlns='http://processSchema.eGastos/'></currencyId><currencyName xmlns='http://processSchema.eGastos/'></currencyName><exchangeRate xmlns='http://processSchema.eGastos/'>0</exchangeRate><initiatorLogin xmlns='http://processSchema.eGastos/'></initiatorLogin><initiatorName xmlns='http://processSchema.eGastos/'></initiatorName><PAClientId xmlns='http://processSchema.eGastos/'></PAClientId><PAClientName xmlns='http://processSchema.eGastos/'></PAClientName><responsibleLogin xmlns='http://processSchema.eGastos/'></responsibleLogin><responsibleName xmlns='http://processSchema.eGastos/'></responsibleName><responsibleEmployeeNum xmlns='http://processSchema.eGastos/'></responsibleEmployeeNum><responsibleUserName xmlns='http://processSchema.eGastos/'></responsibleUserName><responsiblePayMethod xmlns='http://processSchema.eGastos/'></responsiblePayMethod><pasteur xmlns='http://processSchema.eGastos/'>false</pasteur><areaId xmlns='http://processSchema.eGastos/'>0</areaId><areaText xmlns='http://processSchema.eGastos/'></areaText><ultimusNumber xmlns='http://processSchema.eGastos/' >0</ultimusNumber><type xmlns='http://processSchema.eGastos/'>0</type><typeName xmlns='http://processSchema.eGastos/'></typeName><status xmlns='http://processSchema.eGastos/'>0</status><statusName xmlns='http://processSchema.eGastos/'></statusName><salesForce xmlns='http://processSchema.eGastos/' >false</salesForce>";
                                foreach (XmlNode UR in globalNode)
                                {
                                    cont++;
                                    if (UR.Name == "areaId") { UR.InnerText = me.UltRequest.areaId.ToString(); }
                                    if (UR.Name == "areaText") { UR.InnerText = me.UltRequest.areaText; }
                                    if (UR.Name == "arrival") { UR.InnerText = me.UltRequest.arrival; }
                                    if (UR.Name == "CeCoCode") { UR.InnerText = me.UltRequest.CeCoCode.ToString(); }
                                    if (UR.Name == "CeCoMiniCode") { UR.InnerText = me.UltRequest.CeCoMiniCode.ToString(); }
                                    if (UR.Name == "CeCoMiniName") { UR.InnerText = me.UltRequest.CeCoMiniName; }
                                    if (UR.Name == "CeCoName") { UR.InnerText = me.UltRequest.CeCoName; }
                                    if (UR.Name == "companyCode") { UR.InnerText = me.UltRequest.companyCode.ToString(); }
                                    if (UR.Name == "companyName") { UR.InnerText = me.UltRequest.companyName; }
                                    if (UR.Name == "currencyId") { UR.InnerText = me.UltRequest.currencyId; }
                                    if (UR.Name == "currencyName") { UR.InnerText = me.UltRequest.currencyName; }
                                    if (UR.Name == "departureDate") { UR.InnerText = me.UltRequest.departureDate; }
                                    if (UR.Name == "exchangeRate") { UR.InnerText = me.UltRequest.exchangeRate.ToString(); }
                                    if (UR.Name == "idRequest") { UR.InnerText = me.UltRequest.idRequest.ToString(); }
                                    if (UR.Name == "initiatorLogin") { UR.InnerText = me.UltRequest.initiatorLogin; }
                                    if (UR.Name == "initiatorName") { UR.InnerText = me.UltRequest.initiatorName; }
                                    if (UR.Name == "isMiniEvent") { UR.InnerText = me.UltRequest.isMiniEvent.ToString().ToLower(); }
                                    if (UR.Name == "PAClientId") { UR.InnerText = me.UltRequest.PAClientId; }
                                    if (UR.Name == "PAClientName") { UR.InnerText = me.UltRequest.PAClientName; }
                                    if (UR.Name == "pasteur") { UR.InnerText = me.UltRequest.pasteur.ToString().ToLower(); }
                                    if (UR.Name == "PEPElementId") { UR.InnerText = me.UltRequest.PEPElementId; }
                                    if (UR.Name == "PEPElementName") { UR.InnerText = me.UltRequest.PEPElementName; }
                                    if (UR.Name == "requestDate") { UR.InnerText = ToXMLDateFormat(me.UltRequest.requestDate); }
                                    if (UR.Name == "responsibleEmployeeNum") { UR.InnerText = me.UltRequest.responsibleEmployeeNum; }
                                    if (UR.Name == "responsibleLogin") { UR.InnerText = me.UltRequest.responsibleLogin; }
                                    if (UR.Name == "responsibleName") { UR.InnerText = me.UltRequest.responsibleName; }
                                    if (UR.Name == "responsiblePayMethod") { UR.InnerText = me.UltRequest.responsiblePayMethod; }
                                    if (UR.Name == "responsibleUserName") { UR.InnerText = me.UltRequest.responsibleUserName; }
                                    if (UR.Name == "returnDate") { UR.InnerText = me.UltRequest.returnDate; }
                                    if (UR.Name == "salesForce") { UR.InnerText = me.UltRequest.salesForce.ToString().ToLower(); }
                                    if (UR.Name == "status") { UR.InnerText = me.UltRequest.status.ToString(); }
                                    if (UR.Name == "statusName") { UR.InnerText = me.UltRequest.statusName; }
                                    if (UR.Name == "type") { UR.InnerText = me.UltRequest.type.ToString(); }
                                    if (UR.Name == "typeName") { UR.InnerText = me.UltRequest.typeName; }
                                    if (UR.Name == "ultimusNumber") { UR.InnerText = me.UltRequest.ultimusNumber.ToString(); }
                                }
                            }
                        }
                        #endregion

                        #region Global UltRequester
                        if (me.UltRequester != null)
                        {
                            if (globalNode.Name == "UltRequester")
                            {
                                globalNode.InnerXml = "<requesterName xmlns='http://processSchema.eGastos/'> </requesterName><requesterLogin xmlns='http://processSchema.eGastos/'> </requesterLogin><requesterEmail xmlns='http://processSchema.eGastos/'> </requesterEmail><requesterDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requesterDate>";
                                foreach (XmlNode UR in globalNode)
                                {
                                    if (UR.Name == "requesterDate") { UR.InnerText = ToXMLDateFormat(me.UltRequester.requesterDate); }
                                    if (UR.Name == "requesterEmail") { UR.InnerText = me.UltRequester.requesterEmail; }
                                    if (UR.Name == "requesterLogin") { UR.InnerText = me.UltRequester.requesterLogin; }
                                    if (UR.Name == "requesterName") { UR.InnerText = me.UltRequester.requesterName; }
                                }
                            }
                        }
                        #endregion

                        #region Global UltResponsible
                        if (me.UltResponsible != null)
                        {
                            if (globalNode.Name == "UltResponsible")
                            {
                                foreach (XmlNode UR in globalNode)
                                {
                                    if (UR.Name == "responsibleEmail") { UR.InnerText = me.UltResponsible.responsibleEmail; }
                                    if (UR.Name == "responsibleLogin") { UR.InnerText = me.UltResponsible.responsibleLogin; }
                                    if (UR.Name == "responsibleName") { UR.InnerText = me.UltResponsible.responsibleName; }
                                }
                            }
                        }
                        #endregion

                        #region Global UltItinerary[]
                        if ((me.UltItinerary != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.itinerary) && (me.UltItinerary.Length > 0))
                        {

                            if (globalNode.Name == "UltItinerary")
                            {
                                globalNode.InnerXml = xmlUltItinerary;

                                foreach (XmlNode UI in globalNode)
                                {
                                    if (UI.Name == "idItinerary") { UI.InnerText = me.UltItinerary[ui].idItinerary.ToString(); }
                                    if (UI.Name == "idMissionOrder") { UI.InnerText = me.UltItinerary[ui].idMissionOrder.ToString(); }
                                    if (UI.Name == "idConsecutive") { UI.InnerText = me.UltItinerary[ui].idConsecutive.ToString(); }
                                    if (UI.Name == "idLedgerAccount") { UI.InnerText = me.UltItinerary[ui].idLedgerAccount.ToString(); }
                                    if (UI.Name == "nameLedgerAccount") { UI.InnerText = me.UltItinerary[ui].nameLedgerAccount; }
                                    if (UI.Name == "departureHour") { UI.InnerText = me.UltItinerary[ui].departureHour; }
                                    if (UI.Name == "returnHour") { UI.InnerText = me.UltItinerary[ui].returnHour; }
                                    if (UI.Name == "observations") { UI.InnerText = me.UltItinerary[ui].observations; }
                                    if (UI.Name == "travelType") { UI.InnerText = me.UltItinerary[ui].travelType.ToString(); }
                                    if (UI.Name == "nameTravelType") { UI.InnerText = me.UltItinerary[ui].nameTravelType; }
                                    if (UI.Name == "departureCountry") { UI.InnerText = me.UltItinerary[ui].departureCountry; }
                                    if (UI.Name == "departureCity") { UI.InnerText = me.UltItinerary[ui].departureCity; }
                                    if (UI.Name == "arrivalCountry") { UI.InnerText = me.UltItinerary[ui].arrivalCountry; }
                                    if (UI.Name == "arrivalCity") { UI.InnerText = me.UltItinerary[ui].arrivalCity; }
                                    if (UI.Name == "departureDate") { UI.InnerText = ToXMLDateFormat(me.UltItinerary[ui].departureDate); }
                                    if (UI.Name == "arrivalDate") { UI.InnerText = ToXMLDateFormat(me.UltItinerary[ui].arrivalDate); }
                                    if (UI.Name == "status") { UI.InnerText = me.UltItinerary[ui].status.ToString().ToLower(); }
                                }
                                ui++;
                            }
                        }
                        #endregion

                        #region Global UltItineraryOptions[]
                        if ((me.UltItineraryOptions != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.itinerary) && (me.UltItineraryOptions.Length > 0))
                        {
                            if (globalNode.Name == "UltItineraryOptions")
                            {
                                globalNode.InnerXml = xmlUltItineraryOptions;

                                foreach (XmlNode UIO in globalNode)
                                {
                                    if (UIO.Name == "idItineraryOption") { UIO.InnerText = me.UltItineraryOptions[uio].idItineraryOption.ToString(); }
                                    if (UIO.Name == "idMissionOrder") { UIO.InnerText = me.UltItineraryOptions[uio].idMissionOrder.ToString(); }
                                    if (UIO.Name == "idRate") { UIO.InnerText = me.UltItineraryOptions[uio].idRate.ToString(); }
                                    if (UIO.Name == "quoteRate") { UIO.InnerText = me.UltItineraryOptions[uio].quoteRate.ToString(); }
                                    if (UIO.Name == "observations") { UIO.InnerText = me.UltItineraryOptions[uio].observations; }
                                    if (UIO.Name == "confirmed") { UIO.InnerText = me.UltItineraryOptions[uio].confirmed.ToString().ToLower(); }
                                    if (UIO.Name == "lastDayPurchase") { UIO.InnerText = ToXMLDateFormat(me.UltItineraryOptions[uio].lastDayPurchase); }
                                }
                                uio++;
                            }
                        }
                        #endregion

                        #region Global UltItineraryOptionsDetail[]
                        if ((me.UltItineraryOptionsDetail != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.itinerary) && (me.UltItineraryOptionsDetail.Length > 0))
                        {
                            if (globalNode.Name == "UltItineraryOptionsDetail")
                            {
                                globalNode.InnerXml = xmlUltItineraryOptionsDetail;

                                foreach (XmlNode UIOD in globalNode)
                                {
                                    if (UIOD.Name == "airlineFlight") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].airlineFlight; }
                                    if (UIOD.Name == "arrival") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].arrival; }
                                    if (UIOD.Name == "arrivalDate") { UIOD.InnerText = ToXMLDateFormat(me.UltItineraryOptionsDetail[uiod].arrivalDate); }
                                    if (UIOD.Name == "departure") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].departure; }
                                    if (UIOD.Name == "departureDate") { UIOD.InnerText = ToXMLDateFormat(me.UltItineraryOptionsDetail[uiod].departureDate); }
                                    if (UIOD.Name == "idItineraryOption") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].idItineraryOption.ToString(); }
                                    if (UIOD.Name == "idItineraryOptionsDetail") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].idItineraryOptionsDetail.ToString(); }
                                    if (UIOD.Name == "idMissionOrder") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].idMissionOrder.ToString(); }
                                    if (UIOD.Name == "lapseTime") { UIOD.InnerText = me.UltItineraryOptionsDetail[uiod].lapseTime.ToString(); }
                                }
                                uiod++;
                            }
                        }
                        #endregion

                        #region Global UltPAClient[]
                        if ((me.UltPAClient != null) && (me.UltRequest.type >= 3) && (me.UltPAClient.Length > 0))
                        {
                            if (globalNode.Name == "UltPAClient")
                            {
                                globalNode.InnerXml = xmlUltPAClient;

                                foreach (XmlNode UPAC in globalNode)
                                {
                                    if (UPAC.Name == "code") { UPAC.InnerText = me.UltPAClient[pac].code; }
                                    if (UPAC.Name == "idExpenseAccountDetail") { UPAC.InnerText = me.UltPAClient[pac].idExpenseAccountDetail.ToString(); }
                                    if (UPAC.Name == "name") { UPAC.InnerText = me.UltPAClient[pac].name; }
                                }
                                pac++;
                            }
                        }
                        #endregion

                        #region Global UltExpenseAccountDetail[]
                        if ((me.UltExpenseAccountDetail != null) && (me.UltRequest.type >= 3) && (me.UltExpenseAccountDetail.Length > 0))
                        {
                            if (globalNode.Name == "UltExpenseAccountDetail")
                            {
                                globalNode.InnerXml = xmlUltExpenseAccountDetail;

                                foreach (XmlNode UEAD in globalNode)
                                {
                                    if (UEAD.Name == "idExpenseAccountDetail") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idExpenseAccountDetail.ToString(); }
                                    if (UEAD.Name == "idExpenseAccount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idExpenseAccount.ToString(); }
                                    if (UEAD.Name == "expenseDate") { UEAD.InnerText = ToXMLDateFormat(me.UltExpenseAccountDetail[ead].expenseDate); }
                                    if (UEAD.Name == "idAccount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idAccount.ToString(); }
                                    if (UEAD.Name == "accountName") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].accountName; }
                                    if (UEAD.Name == "amount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].amount.ToString(); }
                                    if (UEAD.Name == "invoiceNumber") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].invoiceNumber; }
                                    if (UEAD.Name == "place") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].place; }
                                    if (UEAD.Name == "numberOfDiners") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].numberOfDiners.ToString(); }
                                    if (UEAD.Name == "IVA") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].IVA.ToString(); }
                                    if (UEAD.Name == "healthProfessional") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].healthProfessional.ToString().ToLower(); }
                                    if (UEAD.Name == "discount") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].discount.ToString(); }
                                    if (UEAD.Name == "hasPAClient") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].hasPAClient.ToString().ToLower(); }
                                    if (UEAD.Name == "IVATypeId") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].IVATypeId; }
                                    if (UEAD.Name == "IVATypeName") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].IVATypeName; }
                                    if (UEAD.Name == "total") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].total.ToString(); }
                                    if (UEAD.Name == "observationId") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].observationId.ToString(); }
                                    if (UEAD.Name == "observationName") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].observationName; }
                                    if (UEAD.Name == "idXml") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idXml.ToString(); }
                                    if (UEAD.Name == "amountCFDI") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].amountCFDI.ToString(); }
                                    if (UEAD.Name == "ivaCFDI") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].ivaCFDI.ToString(); }
                                    if (UEAD.Name == "idExtract") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].idExtract.ToString(); }
                                    if (UEAD.Name == "amountExtract") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].amountExtract.ToString(); }
                                    if (UEAD.Name == "conciliated") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].conciliated.ToString().ToLower(); }
                                    if (UEAD.Name == "strike") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].strike.ToString().ToLower(); }
                                    if (UEAD.Name == "status") { UEAD.InnerText = me.UltExpenseAccountDetail[ead].status.ToString().ToLower(); }
                                }
                                ead++;
                            }
                        }
                        #endregion

                    }

                    #region StepSchemaUltHotel
                    if ((me.UltHotel != null) && (me.UltRequest.type < 3) && (me.UltMissionOrder.hotel))
                    {
                        int i = me.UltHotel.Length - 1;
                        foreach (object obj in me.UltHotel)
                        {
                            XmlNode xmlUltHotelElement = ObjXML.CreateNode("element", "StepSchemaUltHotel", "");

                            xmlUltHotelElement.InnerXml = xmlStepSchemaUltHotel;
                            ObjXML.ChildNodes[1].InsertBefore(xmlUltHotelElement, ObjXML.ChildNodes[1].ChildNodes[2]);

                            foreach (XmlNode UH in xmlUltHotelElement)
                            {
                                if (UH.Name == "idHotel") { UH.InnerText = UH.InnerText = me.UltHotel[i].idHotel.ToString(); }
                                if (UH.Name == "idMissionOrder") { UH.InnerText = me.UltHotel[i].idMissionOrder.ToString(); }
                                if (UH.Name == "idRated") { UH.InnerText = me.UltHotel[i].idRated.ToString(); }
                                if (UH.Name == "idConsecutive") { UH.InnerText = me.UltHotel[i].idConsecutive.ToString(); }
                                if (UH.Name == "idLegerAccount") { UH.InnerText = me.UltHotel[i].idLegerAccount.ToString(); }
                                if (UH.Name == "checkInDate") { UH.InnerText = ToXMLDateFormat(me.UltHotel[i].checkInDate); }
                                if (UH.Name == "checkoutDate") { UH.InnerText = ToXMLDateFormat(me.UltHotel[i].checkoutDate); }
                                if (UH.Name == "quotedRate") { UH.InnerText = me.UltHotel[i].quotedRate.ToString(); }
                                if (UH.Name == "realRate") { UH.InnerText = me.UltHotel[i].realRate.ToString(); }
                                if (UH.Name == "IVA") { UH.InnerText = me.UltHotel[i].IVA.ToString(); }
                                if (UH.Name == "hotelTax") { UH.InnerText = me.UltHotel[i].hotelTax.ToString(); }
                                if (UH.Name == "otherTaxes") { UH.InnerText = me.UltHotel[i].otherTaxes.ToString(); }
                                if (UH.Name == "status") { UH.InnerText = me.UltHotel[i].status.ToString().ToLower(); }
                                if (UH.Name == "lineStatus") { UH.InnerText = me.UltHotel[i].lineStatus.ToString(); }
                            }
                            i--;
                        }
                    }
                    #endregion

                    XmlDataDocument ObjXMLSend = new XmlDataDocument();
                    ObjXMLSend.InnerXml = ObjXML.InnerXml.Replace("StepSchemaUltHotel xmlns=\"\"", "StepSchemaUltHotel");

                    bolResultado = ult_obj.LaunchIncident(fd.UserLogin, summary, "Incidente criado por: ", false, 9, ObjXMLSend.InnerXml, true, out intIncident, out strError);
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

         //Descomentar para publicar - Marcio Nakamura
        private ExpenseAccountClient expAccClient = new ExpenseAccountClient();

         //Descomentar para publicar - Marcio Nakamura
        private string generaExpenseAcc(eGastosEntity.Ultimus.UltRequest ultReq, eGastosEntity.Ultimus.UltExpenseAccount ultExpAcc, eGastosEntity.Ultimus.UltExpenseAccountDetail[] ultExpAccDetLst, eGastosEntity.Ultimus.UltPAClient[] ultPACliLst, bool update, int ultNumber)
        {

            expAccClient.initiateVariables(ultReq.requestDate, ultReq.companyName, ultReq.companyCode, ultReq.CeCoCode, ultReq.CeCoMiniCode,
                ultReq.CeCoMiniName, ultReq.isMiniEvent, ultReq.arrival, ultReq.departureDate, ultReq.requestDate.ToString(), ultReq.PEPElementId,
                ultReq.PEPElementName, ultReq.currencyId, ultReq.currencyName, ultReq.initiatorLogin, ultReq.initiatorName, ultReq.responsibleLogin,
                ultReq.responsibleName, ultReq.responsibleEmployeeNum, ultReq.responsibleUserName, ultReq.pasteur, ultReq.areaId,
                ultReq.areaText, (bool)ultReq.salesForce, ultExpAcc.nationalManagerLogin, ultExpAcc.nationalManagerName, ultExpAcc.debitCard
                , ultExpAcc.strike, ultExpAcc.isCFDI);
            if (ultExpAccDetLst != null)
            {
                foreach (eGastosEntity.Ultimus.UltExpenseAccountDetail ultExpAccDet in ultExpAccDetLst)
                {
                    expAccClient.createExpenseAccountDetail(ultExpAccDet.idExpenseAccountDetail, ultExpAccDet.expenseDate, (int)ultExpAccDet.idAccount,
                    ultExpAccDet.accountName, ultExpAccDet.amount, ultExpAccDet.invoiceNumber, ultExpAccDet.place, ultExpAccDet.numberOfDiners,
                    ultExpAccDet.IVA, ultExpAccDet.healthProfessional, ultExpAccDet.hasPAClient, ultExpAccDet.total, ultExpAccDet.observationId,
                    ultExpAccDet.observationName, ultExpAccDet.idXml, ultExpAccDet.amountCFDI, ultExpAccDet.ivaCFDI, ultExpAccDet.idExtract,
                    ultExpAccDet.amountExtract, ultExpAccDet.conciliated, ultExpAccDet.strike);
                }

            }
            if (ultPACliLst != null)
            {
                foreach (eGastosEntity.Ultimus.UltPAClient ultPACli in ultPACliLst)
                {
                    expAccClient.createPAClient(ultPACli.idExpenseAccountDetail, ultPACli.code, ultPACli.name);
                }
            }
            if (!update)
            {
                return expAccClient.validateAndSaveRequest(0);
            }
            else {
                return expAccClient.updateRequest(ultNumber).ToString();
            }

            //return expAccClient.getUltExpenseFlowVariables();

        }

        // Descomentar para publicar - Marcio Nakamura
        private MissionOrderClient misOrdClient = new MissionOrderClient();

        // Descomentar para publicar - Marcio Nakamura
        private string generaMissionOrder(eGastosEntity.Ultimus.UltRequest ultReq, eGastosEntity.Ultimus.UltMissionOrder ultMisOrd, eGastosEntity.Ultimus.UltItinerary[] ultItiLst, eGastosEntity.Ultimus.UltHotel[] ultHotLst, bool update, int ultNumber)
        {

            misOrdClient.initiateVariables(ultReq.requestDate, ultReq.companyName, ultReq.companyCode, ultReq.CeCoCode, ultReq.arrival,
                ultReq.departureDate, ultReq.returnDate, ultReq.PEPElementId, ultReq.PEPElementName, ultReq.currencyId, ultReq.currencyName,
                ultReq.initiatorLogin, ultReq.initiatorName, ultReq.responsibleLogin, ultReq.responsibleName, ultReq.responsibleEmployeeNum,
                ultReq.responsibleUserName, ultReq.pasteur, ultReq.areaId, ultReq.areaText, (bool)ultReq.salesForce, ultMisOrd.travelId,
                ultMisOrd.travelName, ultMisOrd.objective, ultMisOrd.advance, ultMisOrd.comment, ultMisOrd.exceededAdvance, ultMisOrd.missionOrderType,
                ultMisOrd.missionOrderTypeText, ultMisOrd.advanceAndDebitCard);
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
            if (!update)
            {
                return misOrdClient.validateAndSaveRequest();
            }
            else {
                return misOrdClient.updateRequest(ultNumber).ToString();
            }

        }


        [WebMethod]
        public int SendReviewData()
        //public int SendReviewData(Entity.MasterEntity me, Entity.FilterData fd, out string error)
        {
            //DEBUG ------------------------------------------------------------
            Entity.FilterData fd = new Entity.FilterData();
            Entity.MasterEntity me = new eGastosWS.Entity.MasterEntity();
            string error;
            me = ge.me1(ref fd);
            //------------------------------------------------------------------            

            string xmlstr = getUltimusXML(fd);

            XmlDataDocument EAReviewXML = new System.Xml.XmlDataDocument();
            XmlDocument oXmlDoc = new XmlDocument();
            EAReviewXML.LoadXml(xmlstr.Replace("\"", "'"));

            string ProcessVersion = EAReviewXML.ChildNodes.Item(1).Attributes["xmlns"].Value;
            int n = ProcessVersion.LastIndexOf("/");
            string ProcessVersionNumber = ProcessVersion.Substring(0, n).ToString() + "/Types";

            XmlNode XMLNodeGlobal = (EAReviewXML.ChildNodes[1].ChildNodes[0]);
            XmlNode XMLNodeUltApprove = null;
            int nodeCont = XMLNodeGlobal.ChildNodes.Count;
            int nodeCountAH = 0, nodeAHLastIndex = 0, nodeAHFirstIndex = 0; // UltApprovalHistory
            int nodeCountEAD = 0, nodeEADLastIndex = 0, nodeEADFirstIndex = 0; // UltExpenseAccountDetail
            int nodeCountPAC = 0, nodePACLastIndex = 0, nodePACFirstIndex = 0; // UltPAClient

            string nodeName = "";
            XmlNode node = null;

            for (int h = 0; h < nodeCont; h++)
            {
                nodeName = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[h].Name;
                if (nodeName == "UltApprovalHistory")
                {
                    if (nodeAHFirstIndex == 0)
                        nodeAHFirstIndex = h;

                    nodeCountAH++;
                    nodeAHLastIndex = h;
                }
                if (nodeName == "UltExpenseAccountDetail")
                {
                    if (nodeEADFirstIndex == 0)
                        nodeEADFirstIndex = h;

                    nodeCountEAD++;
                    nodeEADLastIndex = h;
                }
                if (nodeName == "UltPAClient")
                {
                    if (nodePACFirstIndex == 0)
                        nodePACFirstIndex = h;

                    nodeCountPAC++;
                    nodePACLastIndex = h;
                }
            }

            #region UltApprovalHistory
            XmlElement xmlUltApprovalHistory = EAReviewXML.CreateElement("UltApprovalHistory", ProcessVersionNumber);
            xmlUltApprovalHistory.InnerXml = "<stepName xmlns=\"http://processSchema.eGastos/\"></stepName><approverName xmlns=\"http://processSchema.eGastos/\"></approverName><approverLogin xmlns=\"http://processSchema.eGastos/\"></approverLogin><userEmail xmlns=\"http://processSchema.eGastos/\"></userEmail><approveDate xmlns=\"http://processSchema.eGastos/\"></approveDate><comments xmlns=\"http://processSchema.eGastos/\"></comments><approveStatus xmlns=\"http://processSchema.eGastos/\"></approveStatus>";
            EAReviewXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltApprovalHistory, EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeAHLastIndex]);
            nodeCont = XMLNodeGlobal.ChildNodes.Count;
            nodeCountAH++;
            int a = 0;

            for (int i = nodeAHFirstIndex; (i < (nodeCountAH + nodeAHLastIndex) + 1) && (a < me.UltApprovalHistory.Length); i++)
            {
                node = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                node.ChildNodes[0].InnerText = me.UltApprovalHistory[a].stepName;
                node.ChildNodes[1].InnerText = me.UltApprovalHistory[a].approverName;
                node.ChildNodes[2].InnerText = me.UltApprovalHistory[a].approverLogin;
                node.ChildNodes[3].InnerText = me.UltApprovalHistory[a].userEmail;
                node.ChildNodes[4].InnerText = ToXMLDateFormat(me.UltApprovalHistory[a].approveDate);
                node.ChildNodes[5].InnerText = me.UltApprovalHistory[a].comments;
                node.ChildNodes[6].InnerText = me.UltApprovalHistory[a].approveStatus;
                a++;
            }
            #endregion

            #region IF Account Expenses
            if (me.UltRequest.type >= 3) // Account Expenses
            {
                XmlElement xmlUltExpenseAccountDetail = EAReviewXML.CreateElement("UltExpenseAccountDetail", ProcessVersionNumber);
                xmlUltExpenseAccountDetail.InnerXml = "<idExpenseAccountDetail xmlns=\"http://processSchema.eGastos/\"></idExpenseAccountDetail><idExpenseAccount xmlns=\"http://processSchema.eGastos/\"></idExpenseAccount><expenseDate xmlns=\"http://processSchema.eGastos/\"></expenseDate><idAccount xmlns=\"http://processSchema.eGastos/\"></idAccount><amount xmlns=\"http://processSchema.eGastos/\"></amount><invoiceNumber xmlns=\"http://processSchema.eGastos/\"></invoiceNumber><place xmlns=\"http://processSchema.eGastos/\"></place><numberOfDiners xmlns=\"http://processSchema.eGastos/\"></numberOfDiners><IVA xmlns=\"http://processSchema.eGastos/\"></IVA><healthProfessional xmlns=\"http://processSchema.eGastos/\"></healthProfessional><discount xmlns=\"http://processSchema.eGastos/\"></discount><hasPAClient xmlns=\"http://processSchema.eGastos/\"></hasPAClient><IVATypeId xmlns=\"http://processSchema.eGastos/\"></IVATypeId><IVATypeName xmlns=\"http://processSchema.eGastos/\"></IVATypeName><total xmlns=\"http://processSchema.eGastos/\"></total><observationId xmlns=\"http://processSchema.eGastos/\"></observationId><observationName xmlns=\"http://processSchema.eGastos/\"></observationName><status xmlns=\"http://processSchema.eGastos/\"></status>";
                EAReviewXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltExpenseAccountDetail, EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodeEADLastIndex]);
                nodeCont = XMLNodeGlobal.ChildNodes.Count;
                nodeCountEAD++;
                int ead = 0;

                for (int i = nodeEADFirstIndex; (i < nodeCountEAD + 1) && (ead < me.UltExpenseAccountDetail.Length); i++)
                {
                    node = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                    node.ChildNodes[0].InnerText = me.UltExpenseAccountDetail[ead].idExpenseAccountDetail.ToString();
                    node.ChildNodes[1].InnerText = me.UltExpenseAccountDetail[ead].idExpenseAccount.ToString();
                    node.ChildNodes[2].InnerText = ToXMLDateFormat(me.UltExpenseAccountDetail[ead].expenseDate);
                    node.ChildNodes[3].InnerText = me.UltExpenseAccountDetail[ead].idAccount.ToString();
                    node.ChildNodes[4].InnerText = me.UltExpenseAccountDetail[ead].accountName;
                    node.ChildNodes[5].InnerText = me.UltExpenseAccountDetail[ead].amount.ToString();
                    node.ChildNodes[6].InnerText = me.UltExpenseAccountDetail[ead].invoiceNumber;
                    node.ChildNodes[7].InnerText = me.UltExpenseAccountDetail[ead].place;
                    node.ChildNodes[8].InnerText = me.UltExpenseAccountDetail[ead].numberOfDiners.ToString();
                    node.ChildNodes[9].InnerText = me.UltExpenseAccountDetail[ead].IVA.ToString();
                    node.ChildNodes[10].InnerText = me.UltExpenseAccountDetail[ead].healthProfessional.ToString().ToLower();
                    node.ChildNodes[11].InnerText = me.UltExpenseAccountDetail[ead].discount.ToString();
                    node.ChildNodes[12].InnerText = me.UltExpenseAccountDetail[ead].hasPAClient.ToString().ToLower();
                    node.ChildNodes[13].InnerText = me.UltExpenseAccountDetail[ead].IVATypeId;
                    node.ChildNodes[14].InnerText = me.UltExpenseAccountDetail[ead].IVATypeName;
                    node.ChildNodes[15].InnerText = me.UltExpenseAccountDetail[ead].total.ToString();
                    node.ChildNodes[16].InnerText = me.UltExpenseAccountDetail[ead].observationId.ToString();
                    node.ChildNodes[17].InnerText = me.UltExpenseAccountDetail[ead].observationName;
                    node.ChildNodes[18].InnerText = me.UltExpenseAccountDetail[ead].status.ToString().ToLower();
                    ead++;
                }

                XmlElement xmlUltPAClient = EAReviewXML.CreateElement("UltPAClient", ProcessVersionNumber);
                xmlUltPAClient.InnerXml = "";
                EAReviewXML.ChildNodes[1].ChildNodes[0].InsertAfter(xmlUltPAClient, EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[nodePACLastIndex]);
                nodeCont = XMLNodeGlobal.ChildNodes.Count;
                nodeCountPAC++;
                int pac = 0;

                for (int i = nodePACFirstIndex; (i < nodeCountPAC + 1) && (pac < me.UltPAClient.Length); i++)
                {
                    node = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                    node.ChildNodes[0].InnerText = me.UltPAClient[pac].idExpenseAccountDetail.ToString();
                    node.ChildNodes[0].InnerText = me.UltPAClient[pac].code;
                    node.ChildNodes[0].InnerText = me.UltPAClient[pac].name;
                }
            }
            #endregion

            for (int i = 0; i < nodeCont; i++)
            {
                nodeName = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i].Name;
                node = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];

                #region UltApprove
                if (nodeName == "UltApprove")
                {
                    XMLNodeUltApprove = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                    node.ChildNodes[0].InnerText = me.UltApprove.approved.ToString().ToLower();
                    node.ChildNodes[1].InnerText = me.UltApprove.approverName;
                    node.ChildNodes[2].InnerText = me.UltApprove.approverLogin;
                    node.ChildNodes[3].InnerText = me.UltApprove.approverEmail;
                }
                #endregion

                #region UltExpenseAccount
                if (nodeName == "UltExpenseAccount" && me.UltRequest.type >= 3)
                {
                    XMLNodeUltExpenseAccount = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                    node.ChildNodes[0].InnerText = me.UltExpenseAccount.idExpenseAccount.ToString();
                    node.ChildNodes[1].InnerText = me.UltExpenseAccount.nationalManagerLogin;
                    node.ChildNodes[2].InnerText = me.UltExpenseAccount.nationalManagerName;
                    node.ChildNodes[3].InnerText = me.UltExpenseAccount.debitCard.ToString().ToLower();
                    node.ChildNodes[4].InnerText = me.UltExpenseAccount.totalMiniEvent.ToString();
                    node.ChildNodes[5].InnerText = me.UltExpenseAccount.totalMeal.ToString();
                    node.ChildNodes[6].InnerText = me.UltExpenseAccount.totalNationalMeal.ToString();
                    node.ChildNodes[7].InnerText = me.UltExpenseAccount.overdue.ToString().ToLower();
                    node.ChildNodes[8].InnerText = me.UltExpenseAccount.charged.ToString().ToLower();
                }
                #endregion

                #region UltExpenseFlowVariables
                if (nodeName == "UltExpenseFlowVariables" && me.UltRequest.type >= 3)
                {
                    XMLNodeUltExpenseFlowVariables = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i];
                    for (int efv = 0; efv < XMLNodeUltExpenseFlowVariables.ChildNodes.Count; efv++)
                    {

                        if (node.ChildNodes[efv].Name == "activeDirAreaGastos")
                        {
                            if (me.UltExpenseFlowVariables.activeDirAreaGastos != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.activeDirAreaGastos.ToString().ToLower();
                        }
                        if (node.ChildNodes[efv].Name == "activeDirFinanzasGastos")
                        {
                            if (me.UltExpenseFlowVariables.activeDirFinanzasGastos != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.activeDirFinanzasGastos.ToString().ToLower();
                        }
                        if (node.ChildNodes[efv].Name == "activeDirGralGastos")
                        {
                            if (me.UltExpenseFlowVariables.activeDirGralGastos != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.activeDirGralGastos.ToString().ToLower();
                        }
                        if (node.ChildNodes[efv].Name == "activeManager")
                        {
                            if (me.UltExpenseFlowVariables.activeManager != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.activeManager.ToString().ToLower();
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionAutorizador1")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionAutorizador1 != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionAutorizador1;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionAutorizador2")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionAutorizador2 != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionAutorizador2;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionAutorizador3")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionAutorizador3 != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionAutorizador3;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionAutorizador4")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionAutorizador4 != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionAutorizador4;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionController1")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionController1 != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionController1;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionController2")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionController2 != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionController2;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionControlling")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionControlling != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionControlling;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionDirAreaGastos")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionDirAreaGastos != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionDirAreaGastos;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionFinanzasGastos")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionFinanzasGastos != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionFinanzasGastos;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionNationalManager")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionNationalManager != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionNationalManager;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionObservador")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionObservador != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionObservador;
                        }
                        if (node.ChildNodes[efv].Name == "jobFunctionResponsible")
                        {
                            if (me.UltExpenseFlowVariables.jobFunctionResponsible != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.jobFunctionResponsible;
                        }
                        if (node.ChildNodes[efv].Name == "summaryText")
                        {
                            if (me.UltExpenseFlowVariables.summaryText != null)
                                node.ChildNodes[efv].InnerText = me.UltExpenseFlowVariables.summaryText;
                        }
                    }
                }
                #endregion

                #region UltRequest
                if (nodeName == "UltRequest" && me.UltRequest.type >= 3)
                {
                    XMLNodeUltRequest = EAReviewXML.ChildNodes[1].ChildNodes[0].ChildNodes[i]; //35

                    for (int r = 0; r < XMLNodeUltRequest.ChildNodes.Count; r++)
                    {
                        if (node.ChildNodes[r].Name == "areaId")
                        {
                            if (me.UltRequest.areaId != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.areaId.ToString();
                        }
                        if (node.ChildNodes[r].Name == "areaText")
                        {
                            if (me.UltRequest.areaText != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.areaText;
                        }
                        if (node.ChildNodes[r].Name == "arrival")
                        {
                            if (me.UltRequest.arrival != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.arrival;
                        }
                        if (node.ChildNodes[r].Name == "CeCoCode")
                        {
                            if (me.UltRequest.CeCoCode != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.CeCoCode.ToString();
                        }
                        if (node.ChildNodes[r].Name == "CeCoMiniCode")
                        {
                            if (me.UltRequest.CeCoMiniCode != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.CeCoMiniCode.ToString();
                        }
                        if (node.ChildNodes[r].Name == "CeCoMiniName")
                        {
                            if (me.UltRequest.CeCoMiniName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.CeCoMiniName;
                        }
                        if (node.ChildNodes[r].Name == "CeCoName")
                        {
                            if (me.UltRequest.CeCoName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.CeCoName;
                        }
                        if (node.ChildNodes[r].Name == "companyCode")
                        {
                            if (me.UltRequest.companyCode != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.companyCode.ToString();
                        }
                        if (node.ChildNodes[r].Name == "companyName")
                        {
                            if (me.UltRequest.companyName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.companyName;
                        }
                        if (node.ChildNodes[r].Name == "currencyId")
                        {
                            if (me.UltRequest.currencyId != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.currencyId;
                        }
                        if (node.ChildNodes[r].Name == "currencyName")
                        {
                            if (me.UltRequest.currencyName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.currencyName;
                        }
                        if (node.ChildNodes[r].Name == "departureDate")
                        {
                            if (me.UltRequest.departureDate != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.departureDate;
                        }
                        if (node.ChildNodes[r].Name == "exchangeRate")
                        {
                            if (me.UltRequest.exchangeRate != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.exchangeRate.ToString();
                        }
                        if (node.ChildNodes[r].Name == "idRequest")
                        {
                            if (me.UltRequest.idRequest != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.idRequest.ToString();
                        }
                        if (node.ChildNodes[r].Name == "initiatorLogin")
                        {
                            if (me.UltRequest.initiatorLogin != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.initiatorLogin;
                        }
                        if (node.ChildNodes[r].Name == "initiatorName")
                        {
                            if (me.UltRequest.initiatorName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.initiatorName;
                        }
                        if (node.ChildNodes[r].Name == "isMiniEvent")
                        {
                            if (me.UltRequest.isMiniEvent != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.isMiniEvent.ToString().ToLower();
                        }
                        if (node.ChildNodes[r].Name == "PAClientId")
                        {
                            if (me.UltRequest.PAClientId != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.PAClientId;
                        }
                        if (node.ChildNodes[r].Name == "PAClientName")
                        {
                            if (me.UltRequest.PAClientName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.PAClientName;
                        }
                        if (node.ChildNodes[r].Name == "pasteur")
                        {
                            if (me.UltRequest.pasteur != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.pasteur.ToString().ToLower();
                        }
                        if (node.ChildNodes[r].Name == "PEPElementId")
                        {
                            if (me.UltRequest.PEPElementId != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.PEPElementId;
                        }
                        if (node.ChildNodes[r].Name == "PEPElementName")
                        {
                            if (me.UltRequest.PEPElementName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.PEPElementName;
                        }
                        if (node.ChildNodes[r].Name == "requestDate")
                        {
                            if (me.UltRequest.requestDate != null)
                                node.ChildNodes[r].InnerText = ToXMLDateFormat(me.UltRequest.requestDate);
                        }
                        if (node.ChildNodes[r].Name == "responsibleEmployeeNum")
                        {
                            if (me.UltRequest.responsibleEmployeeNum != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.responsibleEmployeeNum;
                        }
                        if (node.ChildNodes[r].Name == "responsibleLogin")
                        {
                            if (me.UltRequest.responsibleLogin != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.responsibleLogin;
                        }
                        if (node.ChildNodes[r].Name == "responsibleName")
                        {
                            if (me.UltRequest.responsibleName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.responsibleName;
                        }
                        if (node.ChildNodes[r].Name == "responsiblePayMethod")
                        {
                            if (me.UltRequest.responsiblePayMethod != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.responsiblePayMethod;
                        }
                        if (node.ChildNodes[r].Name == "responsibleUserName")
                        {
                            if (me.UltRequest.responsibleUserName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.responsibleUserName;
                        }
                        if (node.ChildNodes[r].Name == "returnDate")
                        {
                            if (me.UltRequest.returnDate != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.returnDate;
                        }
                        if (node.ChildNodes[r].Name == "salesForce")
                        {
                            if (me.UltRequest.salesForce != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.salesForce.ToString().ToLower();
                        }
                        if (node.ChildNodes[r].Name == "status")
                        {
                            if (me.UltRequest.status != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.status.ToString();
                        }
                        if (node.ChildNodes[r].Name == "statusName")
                        {
                            if (me.UltRequest.statusName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.statusName;
                        }
                        if (node.ChildNodes[r].Name == "type")
                        {
                            if (me.UltRequest.type != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.type.ToString();
                        }
                        if (node.ChildNodes[r].Name == "typeName")
                        {
                            if (me.UltRequest.typeName != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.typeName;
                        }
                        if (node.ChildNodes[r].Name == "ultimusNumber")
                        {
                            if (me.UltRequest.ultimusNumber != null)
                                node.ChildNodes[r].InnerText = me.UltRequest.ultimusNumber.ToString();
                        }
                    }
                }
                #endregion

            }

            XmlNode XmlNodeCustom = (EAReviewXML.ChildNodes[1].ChildNodes[0]);

            int intIncident = fd.IncidentNumber;
            string summary = "";
            string strError = "";

            if (fd.isPasteur)
            {
                ult_objPasteur = new eGastos_Pasteur_BC();
                ult_objPasteur.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, EAReviewXML.InnerXml, true, out strError);
            }
            else
            {
                ult_objPharma = new eGastos_Pharma_BC();
                ult_objPharma.CompleteStep(fd.UserLogin, ref intIncident, fd.StepName, summary, "", false, 9, EAReviewXML.InnerXml, true, out strError);
            }

            error = strError;
            return 0;
        }

    }
}
