using eGastosWS.Entity;
using System;

namespace eGastosWS.Debug
{
    public class Generate
    {
        public MasterEntity me1(ref FilterData fd)
        {
            string Error = "";
            MasterEntity me = new MasterEntity();
            //
            //---------------------------------------------------------
            fd.UserLogin = "adultimus.local/marcio.nakamura";
            fd.isPasteur = false;
            fd.StepName = "Begin";
            fd.ProcessName = "";
            fd.IncidentNumber = 20;
            fd.ErrorMessage = "";
            //---------------------------------------------------------

            #region UltRequest
            me.UltRequest = new eGastosEntity.Ultimus.UltRequest();
            me.UltRequest.areaId = 0;
            me.UltRequest.areaText = "Area TRext";
            me.UltRequest.arrival = "Mexico";
            me.UltRequest.CeCoCode = 4110700;
            me.UltRequest.CeCoMiniCode = 9;
            me.UltRequest.CeCoMiniName = "Ceco um";
            me.UltRequest.companyCode = 2238;
            me.UltRequest.companyName = "Sanofi Aventis de México";
            me.UltRequest.currencyId = "MXN";
            me.UltRequest.currencyName = "Pesos";
            me.UltRequest.departureDate = DateTime.Now.ToString();
            me.UltRequest.exchangeRate = 1;
            me.UltRequest.idRequest = 2000202;
            //me.UltRequest.initiatorLogin = "pharma.aventis.com/e0193822";
            me.UltRequest.initiatorLogin = "MEX-BCAMPOS";
            me.UltRequest.initiatorName = "Campos, Benjamin PH/MX";
            me.UltRequest.isMiniEvent = false;
            me.UltRequest.PAClientId = "0000093109";
            me.UltRequest.PAClientName = "CAMPOS TUN BENJAMIN";
            me.UltRequest.pasteur = fd.isPasteur;
            me.UltRequest.PEPElementId = "PEP012312313";
            me.UltRequest.PEPElementName = "PEP name";
            me.UltRequest.requestDate = DateTime.Now;
            me.UltRequest.responsibleEmployeeNum = "00093109";
            me.UltRequest.responsibleLogin = "pharma.aventis.com/MEX-BCAMPOS";
            me.UltRequest.responsibleName = "BENJAMIN CAMPOS";
            me.UltRequest.responsiblePayMethod = "B";
            me.UltRequest.responsibleUserName = "pharma.aventis.com/MEX-BCAMPOS";
            me.UltRequest.returnDate = DateTime.Now.AddDays(30).ToString();
            me.UltRequest.salesForce = false;
            me.UltRequest.status = 1;
            me.UltRequest.statusName = "PorAutorizar";
            me.UltRequest.type = 2; // <3 = MissionOrder
            me.UltRequest.typeName = "Cuenta de Gastos";
            me.UltRequest.ultimusNumber = 0;
            #endregion

            #region me.UltApprove
            me.UltApprove = new eGastosEntity.Ultimus.UltApprove();
            me.UltApprove.approved = true;
            me.UltApprove.approverEmail = "Sandra.OrtegaM@sanofi.com";
            me.UltApprove.approverLogin = "I0140113";
            me.UltApprove.approverName = "Ortega M, Sandra PH/MX";
            #endregion

            #region UltApprovalHistory
            me.UltApprovalHistory = new eGastosEntity.Ultimus.UltApprovalHistory[1];
            me.UltApprovalHistory[0] = new eGastosEntity.Ultimus.UltApprovalHistory();
            me.UltApprovalHistory[0].approveDate = DateTime.Now.AddDays(-15);
            me.UltApprovalHistory[0].approverLogin = "pharma.aventis.com/e0193822";
            me.UltApprovalHistory[0].approverName = "Marcio Nakamura 123";
            me.UltApprovalHistory[0].approveStatus = "Aprobado";
            me.UltApprovalHistory[0].comments = "Aprovado com sucesso";
            me.UltApprovalHistory[0].stepName = "Begin";
            me.UltApprovalHistory[0].userEmail = "marcio.nakamura@sanofi.com";

            //me.UltApprovalHistory[1] = new eGastosEntity.Ultimus.UltApprovalHistory();
            //me.UltApprovalHistory[1].approveDate = DateTime.Now.AddDays(-10);
            //me.UltApprovalHistory[1].approverLogin = "pharma.aventis.com/e0000001";
            //me.UltApprovalHistory[1].approverName = "Marcio Nakamura 01";
            //me.UltApprovalHistory[1].approveStatus = "Aprobado 01";
            //me.UltApprovalHistory[1].comments = "Aprovado com pouco sucesso 01";
            //me.UltApprovalHistory[1].stepName = "Passo 01";
            //me.UltApprovalHistory[1].userEmail = "marcio.nakamura@sanofi.com01";

            //me.UltApprovalHistory[2] = new eGastosEntity.Ultimus.UltApprovalHistory();
            //me.UltApprovalHistory[2].approveDate = DateTime.Now.AddDays(20);
            //me.UltApprovalHistory[2].approverLogin = "pharma.aventis.com/e0000002";
            //me.UltApprovalHistory[2].approverName = "Marcio Nakamura 002";
            //me.UltApprovalHistory[2].approveStatus = "Aprobado 002";
            //me.UltApprovalHistory[2].comments = "Aprovado com pouco sucesso 002";
            //me.UltApprovalHistory[2].stepName = "Passo 02";
            //me.UltApprovalHistory[2].userEmail = "marcio.nakamura@sanofi.com 002";

            //me.UltApprovalHistory[3] = new eGastosEntity.Ultimus.UltApprovalHistory();
            //me.UltApprovalHistory[3].approveDate = DateTime.Now.AddDays(21);
            //me.UltApprovalHistory[3].approverLogin = "pharma.aventis.com/e0000003";
            //me.UltApprovalHistory[3].approverName = "Marcio Nakamura 003";
            //me.UltApprovalHistory[3].approveStatus = "Aprobado 003";
            //me.UltApprovalHistory[3].comments = "Aprovado com pouco sucesso 003";
            //me.UltApprovalHistory[3].stepName = "Passo 3";
            //me.UltApprovalHistory[3].userEmail = "marcio.nakamura@sanofi.com 003";

            //me.UltApprovalHistory[4] = new eGastosEntity.Ultimus.UltApprovalHistory();
            //me.UltApprovalHistory[4].approveDate = DateTime.Now.AddDays(22);
            //me.UltApprovalHistory[4].approverLogin = "pharma.aventis.com/e0000003";
            //me.UltApprovalHistory[4].approverName = "Marcio Nakamura 003";
            //me.UltApprovalHistory[4].approveStatus = "Aprobado 003";
            //me.UltApprovalHistory[4].comments = "Aprovado com pouco sucesso 003";
            //me.UltApprovalHistory[4].stepName = "Passo 3";
            //me.UltApprovalHistory[4].userEmail = "marcio.nakamura@sanofi.com 003";
            #endregion

            #region me.UltExpenseAccount
            me.UltExpenseAccount = new eGastosEntity.Ultimus.UltExpenseAccount();
            me.UltExpenseAccount.charged = true;
            me.UltExpenseAccount.debitCard = true;
            me.UltExpenseAccount.idExpenseAccount = 123;
            me.UltExpenseAccount.idRequest = 234;
            me.UltExpenseAccount.nationalManagerLogin = "National Manager Login";
            me.UltExpenseAccount.nationalManagerName = "Gerardo Garcia";
            me.UltExpenseAccount.overdue = true;
            me.UltExpenseAccount.totalMeal = 234.56;
            me.UltExpenseAccount.totalMiniEvent = 345.67;
            me.UltExpenseAccount.totalNationalMeal = 456.89;
            me.UltExpenseAccount.strike = true;
            me.UltExpenseAccount.isCFDI = true;
            #endregion

            #region me.UltExpenseAccountDetail
            me.UltExpenseAccountDetail = new eGastosEntity.Ultimus.UltExpenseAccountDetail[2];
            me.UltExpenseAccountDetail[0] = new eGastosEntity.Ultimus.UltExpenseAccountDetail();
            me.UltExpenseAccountDetail[0].accountName = "Account Name";
            me.UltExpenseAccountDetail[0].amount = 123.45;
            me.UltExpenseAccountDetail[0].discount = 234.56;
            me.UltExpenseAccountDetail[0].expenseDate = DateTime.Now;
            me.UltExpenseAccountDetail[0].hasPAClient = true;
            me.UltExpenseAccountDetail[0].healthProfessional = false;
            me.UltExpenseAccountDetail[0].idAccount = 234;
            me.UltExpenseAccountDetail[0].idExpenseAccount = 345;
            me.UltExpenseAccountDetail[0].idExpenseAccountDetail = 456;
            me.UltExpenseAccountDetail[0].invoiceNumber = "12345";
            me.UltExpenseAccountDetail[0].IVA = 123.45;
            me.UltExpenseAccountDetail[0].IVATypeId = "987654321";
            me.UltExpenseAccountDetail[0].IVATypeName = "567890";
            me.UltExpenseAccountDetail[0].numberOfDiners = 3;
            me.UltExpenseAccountDetail[0].observationId = 345;
            me.UltExpenseAccountDetail[0].observationName = "Observador Marcio";
            me.UltExpenseAccountDetail[0].place = "São Paulo";
            me.UltExpenseAccountDetail[0].status = true;
            me.UltExpenseAccountDetail[0].total = 987.65;

            me.UltExpenseAccountDetail[1] = new eGastosEntity.Ultimus.UltExpenseAccountDetail();
            me.UltExpenseAccountDetail[1].accountName = "2 Account Name";
            me.UltExpenseAccountDetail[1].amount = 2345.67;
            me.UltExpenseAccountDetail[1].discount = 345.78;
            me.UltExpenseAccountDetail[1].expenseDate = DateTime.Now;
            me.UltExpenseAccountDetail[1].hasPAClient = false;
            me.UltExpenseAccountDetail[1].healthProfessional = true;
            me.UltExpenseAccountDetail[1].idAccount = 2345;
            me.UltExpenseAccountDetail[1].idExpenseAccount = 3456;
            me.UltExpenseAccountDetail[1].idExpenseAccountDetail = 4567;
            me.UltExpenseAccountDetail[1].invoiceNumber = "987612345";
            me.UltExpenseAccountDetail[1].IVA = 223.45;
            me.UltExpenseAccountDetail[1].IVATypeId = "45687632";
            me.UltExpenseAccountDetail[1].IVATypeName = "asd 567890";
            me.UltExpenseAccountDetail[1].numberOfDiners = 5;
            me.UltExpenseAccountDetail[1].observationId = 3456;
            me.UltExpenseAccountDetail[1].observationName = "2 Observador Marcio";
            me.UltExpenseAccountDetail[1].place = "São Paulo";
            me.UltExpenseAccountDetail[1].status = false;
            me.UltExpenseAccountDetail[1].total = 1987.65;
            #endregion

            #region me.UltExpenseFlowVariables
            me.UltExpenseFlowVariables = new eGastosEntity.Ultimus.UltExpenseFlowVariables();
            me.UltExpenseFlowVariables.activeDirAreaGastos = false;
            me.UltExpenseFlowVariables.activeDirFinanzasGastos = false;
            me.UltExpenseFlowVariables.activeDirGralGastos = false;
            me.UltExpenseFlowVariables.activeManager = true;
            me.UltExpenseFlowVariables.jobFunctionAutorizador1 = "JF:org=Business Organization, dept=AR_LATAM, jf=VICEPRESIDENT_LATAM";
            me.UltExpenseFlowVariables.jobFunctionAutorizador2 = "JF:org=Business Organization, dept=Directores_Pasteur, jf=1";
            me.UltExpenseFlowVariables.jobFunctionAutorizador3 = "";
            me.UltExpenseFlowVariables.jobFunctionAutorizador4 = "";
            me.UltExpenseFlowVariables.jobFunctionController1 = "JF:org=Business Organization, dept=SANOFI PASTEUR, jf=48238055";
            me.UltExpenseFlowVariables.jobFunctionController2 = "";
            me.UltExpenseFlowVariables.jobFunctionControlling = "";
            me.UltExpenseFlowVariables.jobFunctionDirAreaGastos = "";
            me.UltExpenseFlowVariables.jobFunctionFinanzasGastos = "";
            me.UltExpenseFlowVariables.jobFunctionNationalManager = "";
            me.UltExpenseFlowVariables.jobFunctionObservador = "";
            me.UltExpenseFlowVariables.jobFunctionResponsible = "JF:org=Business Organization, dept=SANOFI PASTEUR, jf=405421";
            me.UltExpenseFlowVariables.summaryText = "O.M. para";
            #endregion

            #region me.UltFlobotVariables
            //me.UltFlobotVariables = new WebApplication1.localhost.UltFlobotVariables();
            #endregion

            #region me.UltHotel
            me.UltHotel = new eGastosEntity.Ultimus.UltHotel[2];
            me.UltHotel[0] = new eGastosEntity.Ultimus.UltHotel();
            me.UltHotel[0].address = "Endereco do Hotel";
            me.UltHotel[0].checkInDate = DateTime.Now.AddDays(30);
            me.UltHotel[0].checkoutDate = DateTime.Now.AddDays(35);
            me.UltHotel[0].city = "Sao Paulo 123";
            me.UltHotel[0].country = "Brasil";
            me.UltHotel[0].hotelName = "Transamerica Hotel";
            me.UltHotel[0].hotelTax = 123.45;
            me.UltHotel[0].idConsecutive = 12;
            me.UltHotel[0].idHotel = 11;
            me.UltHotel[0].idLegerAccount = 45;
            me.UltHotel[0].idMissionOrder = 56;
            me.UltHotel[0].idRated = 74;
            me.UltHotel[0].IVA = 789.45;
            me.UltHotel[0].lineStatus = 54;
            me.UltHotel[0].lineStatusName = "Cinquenta e quatro";
            me.UltHotel[0].nameLegerAccount = "Leger Account Name";
            me.UltHotel[0].observations = "Comentarios e observacoes";
            me.UltHotel[0].otherTaxes = 123.45;
            me.UltHotel[0].quotedRate = 432.76;
            me.UltHotel[0].realRate = 82.68;
            me.UltHotel[0].reservation = "Reservation Text";
            me.UltHotel[0].status = false;
            me.UltHotel[0].telephone = "+55(11)3759-6934";

            me.UltHotel[1] = new eGastosEntity.Ultimus.UltHotel();
            me.UltHotel[1].address = "123Endereco do Hotel";
            me.UltHotel[1].checkInDate = DateTime.Now.AddDays(31);
            me.UltHotel[1].checkoutDate = DateTime.Now.AddDays(36);
            me.UltHotel[1].city = "Sao Paulo 234";
            me.UltHotel[1].country = "Brasil 2";
            me.UltHotel[1].hotelName = "Transamerica Hotel 4";
            me.UltHotel[1].hotelTax = 124.45;
            me.UltHotel[1].idConsecutive = 14;
            me.UltHotel[1].idHotel = 12;
            me.UltHotel[1].idLegerAccount = 455;
            me.UltHotel[1].idMissionOrder = 57;
            me.UltHotel[1].idRated = 743;
            me.UltHotel[1].IVA = 772.45;
            me.UltHotel[1].lineStatus = 53;
            me.UltHotel[1].lineStatusName = "Cinquenta e tres";
            me.UltHotel[1].nameLegerAccount = "2 Leger Account Name";
            me.UltHotel[1].observations = "2 Comentarios e observacoes";
            me.UltHotel[1].otherTaxes = 234.56;
            me.UltHotel[1].quotedRate = 873.23;
            me.UltHotel[1].realRate = 83.68;
            me.UltHotel[1].reservation = "2 Reservation Text";
            me.UltHotel[1].status = true;
            me.UltHotel[1].telephone = "+55(11)3759-6934";
            #endregion

            #region me.UltItinerary
            me.UltItinerary = new eGastosEntity.Ultimus.UltItinerary[2];
            me.UltItinerary[0] = new eGastosEntity.Ultimus.UltItinerary();

            me.UltItinerary[0].arrivalCity = "Cidade Destino 1";
            me.UltItinerary[0].arrivalCountry = "Pais Destino 1";
            me.UltItinerary[0].arrivalDate = DateTime.Now.AddDays(32);
            me.UltItinerary[0].departureCity = "Cidade Origem";
            me.UltItinerary[0].departureCountry = "Pais Origem";
            me.UltItinerary[0].departureDate = DateTime.Now.AddDays(31);
            me.UltItinerary[0].departureHour = "23:32";
            me.UltItinerary[0].idConsecutive = 2;
            me.UltItinerary[0].idItinerary = 1;
            me.UltItinerary[0].idLedgerAccount = 3;
            me.UltItinerary[0].idMissionOrder = 4;
            me.UltItinerary[0].nameLedgerAccount = "Name Ledger Account";
            me.UltItinerary[0].nameTravelType = "Travel Type";
            me.UltItinerary[0].observations = "Observacao";
            me.UltItinerary[0].returnHour = "11:54";
            me.UltItinerary[0].status = false;
            me.UltItinerary[0].travelType = 3; // "Particular";

            me.UltItinerary[1] = new eGastosEntity.Ultimus.UltItinerary();
            me.UltItinerary[1].arrivalCity = "Cidade Destino 2";
            me.UltItinerary[1].arrivalCountry = "Pais Destino 2";
            me.UltItinerary[1].arrivalDate = DateTime.Now.AddDays(32);
            me.UltItinerary[1].departureCity = "Cidade Origem 2";
            me.UltItinerary[1].departureCountry = "Pais Origem 2";
            me.UltItinerary[1].departureDate = DateTime.Now.AddDays(31);
            me.UltItinerary[1].departureHour = "22:32";
            me.UltItinerary[1].idConsecutive = 2;
            me.UltItinerary[1].idItinerary = 2;
            me.UltItinerary[1].idLedgerAccount = 3;
            me.UltItinerary[1].idMissionOrder = 4;
            me.UltItinerary[1].nameLedgerAccount = "2 Name Ledger Account";
            me.UltItinerary[1].nameTravelType = "2 Travel Type";
            me.UltItinerary[1].observations = " 2Observacao";
            me.UltItinerary[1].returnHour = "11:55";
            me.UltItinerary[1].status = true;
            me.UltItinerary[1].travelType = 4;
            #endregion


            #region me.UltItineraryOptions
            me.UltItineraryOptions = new eGastosEntity.Ultimus.UltItineraryOptions[1];
            me.UltItineraryOptions[0] = new eGastosEntity.Ultimus.UltItineraryOptions();

            me.UltItineraryOptions[0].confirmed = true;
            me.UltItineraryOptions[0].idItineraryOption = 1;
            me.UltItineraryOptions[0].idMissionOrder = 2;
            me.UltItineraryOptions[0].idRate = 3;
            me.UltItineraryOptions[0].lastDayPurchase = DateTime.Now.AddDays(10);
            me.UltItineraryOptions[0].observations = "Observacao";
            me.UltItineraryOptions[0].quoteRate = 123.45;
            #endregion

            #region me.UltItineraryOptionsDetail
            me.UltItineraryOptionsDetail = new eGastosEntity.Ultimus.UltItineraryOptionsDetail[2];
            me.UltItineraryOptionsDetail[0] = new eGastosEntity.Ultimus.UltItineraryOptionsDetail();
            me.UltItineraryOptionsDetail[0].airlineFlight = "Avianca";
            me.UltItineraryOptionsDetail[0].arrival = "Destino";
            me.UltItineraryOptionsDetail[0].arrivalDate = DateTime.Now.AddDays(28);
            me.UltItineraryOptionsDetail[0].departure = "Aeroporto Internacional de São Paulo";
            me.UltItineraryOptionsDetail[0].departureDate = DateTime.Now.AddDays(12);
            me.UltItineraryOptionsDetail[0].idItineraryOption = 234;
            me.UltItineraryOptionsDetail[0].idItineraryOptionsDetail = 1;
            me.UltItineraryOptionsDetail[0].idMissionOrder = 2;
            me.UltItineraryOptionsDetail[0].lapseTime = 54.21;

            me.UltItineraryOptionsDetail[1] = new eGastosEntity.Ultimus.UltItineraryOptionsDetail();
            me.UltItineraryOptionsDetail[1].airlineFlight = "Avianca";
            me.UltItineraryOptionsDetail[1].arrival = "Destino";
            me.UltItineraryOptionsDetail[1].arrivalDate = DateTime.Now.AddDays(28);
            me.UltItineraryOptionsDetail[1].departure = "Aeroporto Internacional de São Paulo";
            me.UltItineraryOptionsDetail[1].departureDate = DateTime.Now.AddDays(12);
            me.UltItineraryOptionsDetail[1].idItineraryOption = 234;
            me.UltItineraryOptionsDetail[1].idItineraryOptionsDetail = 2;
            me.UltItineraryOptionsDetail[1].idMissionOrder = 2;
            me.UltItineraryOptionsDetail[1].lapseTime = 54.21;
            #endregion

            #region me.UltItineraryRate
            me.UltItineraryRate = new eGastosEntity.Ultimus.UltItineraryRate();
            me.UltItineraryRate.IdItineraryRate = 2;
            me.UltItineraryRate.idMissionOrder = 1;
            me.UltItineraryRate.IVA = 23.45;
            me.UltItineraryRate.lineStatus = 4;
            me.UltItineraryRate.lineStatusName = "quatro";
            me.UltItineraryRate.otherTaxes = 23.45;
            me.UltItineraryRate.realRate = 34.56;
            me.UltItineraryRate.TUA = 67.89;
            #endregion

            #region  me.UltMissionOrder
            me.UltMissionOrder = new eGastosEntity.Ultimus.UltMissionOrder();
            me.UltMissionOrder.advance = 123.45;
            me.UltMissionOrder.advanceApply = true;
            me.UltMissionOrder.comment = "aprovado";
            me.UltMissionOrder.countAgencyWait = 34;
            me.UltMissionOrder.hotel = true;
            me.UltMissionOrder.idAgencyLog = 3;
            me.UltMissionOrder.idAgencyResponse = 4;
            me.UltMissionOrder.idMissionOrder = 1;
            me.UltMissionOrder.idRequest = 2;
            me.UltMissionOrder.itinerary = true;
            me.UltMissionOrder.nationalCurrency = 34.56;
            me.UltMissionOrder.objective = "Objetivo Texto";
            me.UltMissionOrder.statusAgencyProcess = 6;
            me.UltMissionOrder.statusAgencySend = 1;
            me.UltMissionOrder.travelId = "123";
            me.UltMissionOrder.travelName = "A negocios";
            me.UltMissionOrder.advanceAndDebitCard = true; //new
            me.UltMissionOrder.exceededAdvance = true; //new
            me.UltMissionOrder.missionOrderType = 23; // new
            me.UltMissionOrder.missionOrderTypeText = "Vinte e Tres"; // new

            #endregion

            #region me.UltPAClient
            me.UltPAClient = new eGastosEntity.Ultimus.UltPAClient[2];
            me.UltPAClient[0] = new eGastosEntity.Ultimus.UltPAClient();
            me.UltPAClient[0].code = "00001";
            me.UltPAClient[0].idExpenseAccountDetail = 1234;
            me.UltPAClient[0].name = "um dois tres quatro";

            me.UltPAClient[1] = new eGastosEntity.Ultimus.UltPAClient();
            me.UltPAClient[1].code = "00002";
            me.UltPAClient[1].idExpenseAccountDetail = 2345;
            me.UltPAClient[1].name = "dois tres quatro cinco";
            #endregion

            #region me.UltRequester
            me.UltRequester = new eGastosEntity.Ultimus.UltRequester();
            me.UltRequester.requesterDate = DateTime.Now.AddDays(1);
            me.UltRequester.requesterEmail = "requester.email@sanofi.com";
            me.UltRequester.requesterLogin = "pharma.aventis.com/e0193822";
            me.UltRequester.requesterName = "Marcio Nakamura ";
            #endregion

            #region me.UltResponsible
            me.UltResponsible = new eGastosEntity.Ultimus.UltResponsible();
            me.UltResponsible.responsibleEmail = "Jorge.Ortiz@sanofi.com";
            me.UltResponsible.responsibleLogin = "pharma.aventis.com/E0034825";
            me.UltResponsible.responsibleName = "Jorge Ortiz";
            #endregion

            #region me.UltSAPResponse
            me.UltSAPResponse = new eGastosEntity.Ultimus.UltSAPResponse[1];
            me.UltSAPResponse[0] = new eGastosEntity.Ultimus.UltSAPResponse();

            me.UltSAPResponse[0].company = 1;
            me.UltSAPResponse[0].docNumber = 456;
            me.UltSAPResponse[0].idRequest = 2;
            me.UltSAPResponse[0].idResponse = 3;
            me.UltSAPResponse[0].type = "Tipo do Responsavel";
            me.UltSAPResponse[0].year = 2014;
            #endregion

            #region me.UltGetThere
            me.UltGetThere = new eGastosEntity.Ultimus.UltGetThere();
            me.UltGetThere.idGetThere =1;
            me.UltGetThere.idMissionOrder = 2;
            me.UltGetThere.conceptId = 3;
            me.UltGetThere.conceptText = "Texto concept";
            me.UltGetThere.lowCost = true;
            me.UltGetThere.justification = "Justificativa";
            me.UltGetThere.cheapestRate = "Cheapes Rate";
            me.UltGetThere.outPolitic = true;
            me.UltGetThere.outPoliticMessage = "Esta fora da politica";
            #endregion


            return me;
        }
    }
}
