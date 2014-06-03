using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;

namespace eGastosWS.Pharma_XML
{
    public class Class1
    {

        public void pharma()
    {
        XmlDocument ObjXML = null;
        StringBuilder LarXML = null;
        string ProcessVersion = "" , ProcessVersionNumber = "";
        XmlNode XmlNodeCustom = (ObjXML.ChildNodes[1].ChildNodes[0]);

        //LarXML.Append("<?xml version='1.0' encoding='utf-16'?> ");
        //LarXML.Append("<TaskData xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='" + ProcessVersion + "'>");

        //LarXML.Append("  <Global>");
        //LarXML.Append("    <UltConfiguration xmlns='" + ProcessVersionNumber + "'>");
        //LarXML.Append("      <serverName xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/</serverName>");
        //LarXML.Append("      <appDirectory xmlns='http://processSchema.eGastos/'>eGastos/Pages/</appDirectory>");
        //LarXML.Append("      <pageRequest xmlns='http://processSchema.eGastos/'>frmExpenseRequest.aspx</pageRequest>");
        //LarXML.Append("      <pageApproval xmlns='http://processSchema.eGastos/'>frmExpenseApproval.aspx</pageApproval>");
        //LarXML.Append("      <pageExpenseAccount xmlns='http://processSchema.eGastos/'>frmExpenseAccount.aspx</pageExpenseAccount>");
        //LarXML.Append("      <pageConfirmQuote xmlns='http://processSchema.eGastos/'>frmConfirmQuote.aspx</pageConfirmQuote>");
        //LarXML.Append("      <uRLRequest xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseRequest.aspx</uRLRequest>");
        //LarXML.Append("      <uRLApproval xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseApproval.aspx</uRLApproval>");
        //LarXML.Append("      <uRLExpenseAccount xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmExpenseAccount.aspx</uRLExpenseAccount>");
        //LarXML.Append("      <uRLConfirmQuote xmlns='http://processSchema.eGastos/'>http://saosultuat01.pharma.aventis.com/eGastos/Pages/frmConfirmQuote.aspx</uRLConfirmQuote>");
        //LarXML.Append("    </UltConfiguration>");
        //if (me.UltApprovalHistory != null)
        //{
        //    foreach (UltApprovalHistory obj in me.UltApprovalHistory)
        //    {
        //        LarXML.Append("    <UltApprovalHistory xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <stepName xmlns='http://processSchema.eGastos/'>" + obj.stepName + "</stepName>");
        //        LarXML.Append("      <approverName xmlns='http://processSchema.eGastos/'>" + obj.approverName + "</approverName>");
        //        LarXML.Append("      <approverLogin xmlns='http://processSchema.eGastos/'>" + obj.approverLogin + "</approverLogin>");
        //        LarXML.Append("      <userEmail xmlns='http://processSchema.eGastos/'>" + obj.userEmail + "</userEmail>");
        //        LarXML.Append("      <approveDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.approveDate) + "</approveDate>");
        //        LarXML.Append("      <comments xmlns='http://processSchema.eGastos/'>" + obj.comments + "</comments>");
        //        LarXML.Append("      <approveStatus xmlns='http://processSchema.eGastos/'>" + obj.approveStatus + "</approveStatus>");
        //        LarXML.Append("    </UltApprovalHistory>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("    <UltApprovalHistory xmlns='" + ProcessVersionNumber + "'> ");
        //    LarXML.Append("      <approveDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</approveDate> ");
        //    LarXML.Append("    </UltApprovalHistory>");
        //}

        //if (me.UltApprove != null)
        //{
        //    LarXML.Append("    <UltApprove xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("         <approved xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approved.ToString().ToLower() + "</approved>");
        //    LarXML.Append("         <approverName xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverName + "</approverName>");
        //    LarXML.Append("         <approverLogin xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverLogin + "</approverLogin>");
        //    LarXML.Append("         <approverEmail xmlns='http://processSchema.eGastos/'>" + me.UltApprove.approverEmail + "</approverEmail>");
        //    LarXML.Append("    </UltApprove>");
        //}
        //else
        //{
        //    LarXML.Append("    <UltApprove xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("         <approved xmlns='http://processSchema.eGastos/'>false</approved>");
        //    LarXML.Append("    </UltApprove>");

        //}

        //if (me.UltExpenseAccount != null)
        //{
        //    LarXML.Append("    <UltExpenseAccount xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <idExpenseAccount xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.idExpenseAccount + "</idExpenseAccount>");
        //    LarXML.Append("      <nationalManagerLogin xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.nationalManagerLogin + "</nationalManagerLogin>");
        //    LarXML.Append("      <nationalManagerName xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.nationalManagerName + "</nationalManagerName>");
        //    LarXML.Append("      <debitCard xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.debitCard.ToString().ToLower() + "</debitCard>");
        //    LarXML.Append("      <totalMiniEvent xmlns='http://processSchema.eGastos/'>" + (double)me.UltExpenseAccount.totalMiniEvent + "</totalMiniEvent>");
        //    LarXML.Append("      <totalMeal xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.totalMeal + "</totalMeal>");
        //    LarXML.Append("      <totalNationalMeal xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.totalNationalMeal + "</totalNationalMeal>");
        //    LarXML.Append("      <overdue xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.overdue.ToString().ToLower() + "</overdue>");
        //    LarXML.Append("      <charged xmlns='http://processSchema.eGastos/'>" + me.UltExpenseAccount.charged.ToString().ToLower() + "</charged>");
        //    LarXML.Append("    </UltExpenseAccount>");
        //}
        //else
        //{
        //    LarXML.Append("    <UltExpenseAccount xmlns='" + ProcessVersionNumber + "'> ");
        //    LarXML.Append("          <idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount> ");
        //    LarXML.Append("          <creditCard xmlns='http://processSchema.eGastos/'>false</creditCard> ");
        //    LarXML.Append("          <totalMiniEvent xmlns='http://processSchema.eGastos/'>0</totalMiniEvent> ");
        //    LarXML.Append("          <totalMeal xmlns='http://processSchema.eGastos/'>0</totalMeal> ");
        //    LarXML.Append("          <totalNationalMeal xmlns='http://processSchema.eGastos/'>0</totalNationalMeal> ");
        //    LarXML.Append("          <overdue xmlns='http://processSchema.eGastos/'>false</overdue> ");
        //    LarXML.Append("          <charged xmlns='http://processSchema.eGastos/'>false</charged> ");
        //    LarXML.Append("    </UltExpenseAccount> ");

        //}
        //if (me.UltExpenseAccountDetail != null)
        //{
        //    foreach (eGastosEntity.Ultimus.UltExpenseAccountDetail obj in me.UltExpenseAccountDetail)
        //    {
        //        LarXML.Append("    <UltExpenseAccountDetail xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccountDetail + "</idExpenseAccountDetail>");
        //        LarXML.Append("      <idExpenseAccount xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccount + "</idExpenseAccount>");
        //        LarXML.Append("      <expenseDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.expenseDate) + "</expenseDate>");
        //        LarXML.Append("      <idAccount xmlns='http://processSchema.eGastos/'>" + obj.idAccount + "</idAccount>");
        //        LarXML.Append("      <amount xmlns='http://processSchema.eGastos/'>" + obj.amount + "</amount>");
        //        LarXML.Append("      <invoiceNumber xmlns='http://processSchema.eGastos/'>" + obj.invoiceNumber + "</invoiceNumber>");
        //        LarXML.Append("      <place xmlns='http://processSchema.eGastos/'>" + obj.place + "</place>");
        //        LarXML.Append("      <numberOfDiners xmlns='http://processSchema.eGastos/'>" + obj.numberOfDiners + "</numberOfDiners>");
        //        LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + obj.IVA + "</IVA>");
        //        LarXML.Append("      <healthProfessional xmlns='http://processSchema.eGastos/'>" + obj.healthProfessional.ToString().ToLower() + "</healthProfessional>");
        //        LarXML.Append("      <discount xmlns='http://processSchema.eGastos/'>" + obj.discount + "</discount>");
        //        LarXML.Append("      <hasPAClient xmlns='http://processSchema.eGastos/'>" + obj.hasPAClient.ToString().ToLower() + "</hasPAClient>");
        //        LarXML.Append("      <IVATypeId xmlns='http://processSchema.eGastos/'>" + obj.IVATypeId + "</IVATypeId>");
        //        LarXML.Append("      <IVATypeName xmlns='http://processSchema.eGastos/'>" + obj.IVATypeName + "</IVATypeName>");
        //        LarXML.Append("      <total xmlns='http://processSchema.eGastos/'>" + obj.total + "</total>");
        //        LarXML.Append("      <observationId xmlns='http://processSchema.eGastos/'>" + obj.observationId + "</observationId>");
        //        LarXML.Append("      <observationName xmlns='http://processSchema.eGastos/'>" + obj.observationName + "</observationName>");
        //        LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
        //        LarXML.Append("    </UltExpenseAccountDetail>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("    <UltExpenseAccountDetail xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("          <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail>");
        //    LarXML.Append("          <idExpenseAccount xmlns='http://processSchema.eGastos/'>0</idExpenseAccount>");
        //    LarXML.Append("          <expenseDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</expenseDate>");
        //    LarXML.Append("          <idAccount xmlns='http://processSchema.eGastos/'>0</idAccount>");
        //    LarXML.Append("          <amount xmlns='http://processSchema.eGastos/'>0</amount>");
        //    LarXML.Append("          <numberOfDiners xmlns='http://processSchema.eGastos/'>0</numberOfDiners>");
        //    LarXML.Append("          <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
        //    LarXML.Append("          <healthProfessional xmlns='http://processSchema.eGastos/'>false</healthProfessional>");
        //    LarXML.Append("          <discount xmlns='http://processSchema.eGastos/'>0</discount>");
        //    LarXML.Append("          <hasPAClient xmlns='http://processSchema.eGastos/'>false</hasPAClient>");
        //    LarXML.Append("          <total xmlns='http://processSchema.eGastos/'>0</total>");
        //    LarXML.Append("          <observationId xmlns='http://processSchema.eGastos/'>0</observationId>");
        //    LarXML.Append("          <status xmlns='http://processSchema.eGastos/'>false</status>");
        //    LarXML.Append("    </UltExpenseAccountDetail>");
        //}

        //if (me.UltExpenseFlowVariables != null)
        //{
        //    LarXML.Append("    <UltExpenseFlowVariables xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <summaryText xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.summaryText + "</summaryText>");
        //    LarXML.Append("      <jobFunctionResponsible xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionResponsible + "</jobFunctionResponsible>");
        //    LarXML.Append("      <activeDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirAreaGastos.ToString().ToLower() + "</activeDirAreaGastos>");
        //    LarXML.Append("      <activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirFinanzasGastos.ToString().ToLower() + "</activeDirFinanzasGastos>");
        //    LarXML.Append("      <activeManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeManager.ToString().ToLower() + "</activeManager>");
        //    LarXML.Append("      <jobFunctionControlling xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionControlling + "</jobFunctionControlling>");
        //    LarXML.Append("      <jobFunctionNationalManager xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionNationalManager + "</jobFunctionNationalManager>");
        //    LarXML.Append("      <jobFunctionAutorizador1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador1 + "</jobFunctionAutorizador1>");
        //    LarXML.Append("      <jobFunctionAutorizador2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador2 + "</jobFunctionAutorizador2>");
        //    LarXML.Append("      <jobFunctionAutorizador3 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador3 + "</jobFunctionAutorizador3>");
        //    LarXML.Append("      <jobFunctionAutorizador4 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionAutorizador4 + "</jobFunctionAutorizador4>");
        //    LarXML.Append("      <jobFunctionController1 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController1 + "</jobFunctionController1>");
        //    LarXML.Append("      <jobFunctionController2 xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionController2 + "</jobFunctionController2>");
        //    LarXML.Append("      <jobFunctionObservador xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionObservador + "</jobFunctionObservador>");
        //    LarXML.Append("      <jobFunctionDirAreaGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionDirAreaGastos + "</jobFunctionDirAreaGastos>");
        //    LarXML.Append("      <jobFunctionFinanzasGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.jobFunctionFinanzasGastos + "</jobFunctionFinanzasGastos>");
        //    LarXML.Append("      <activeDirGralGastos xmlns='http://processSchema.eGastos/'>" + me.UltExpenseFlowVariables.activeDirGralGastos.ToString().ToLower() + "</activeDirGralGastos>");
        //    LarXML.Append("    </UltExpenseFlowVariables>");
        //}
        //else
        //{
        //    LarXML.Append("    <UltExpenseFlowVariables xmlns='" + ProcessVersionNumber + "'> ");
        //    LarXML.Append("    <activeDirAreaGastos xmlns='http://processSchema.eGastos/'>false</activeDirAreaGastos> ");
        //    LarXML.Append("    <activeDirFinanzasGastos xmlns='http://processSchema.eGastos/'>false</activeDirFinanzasGastos> ");
        //    LarXML.Append("    <activeManager xmlns='http://processSchema.eGastos/'>false</activeManager>");
        //    LarXML.Append("    <activeDirGralGastos xmlns='http://processSchema.eGastos/'>false</activeDirGralGastos> ");
        //    LarXML.Append("    </UltExpenseFlowVariables> ");
        //}

        //if (me.UltFlobotVariables != null)
        //{
        //    LarXML.Append("    <UltFlobotVariables xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <statusAgencyFlobot xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.statusAgencyFlobot + "</statusAgencyFlobot>");
        //    LarXML.Append("      <messageErrorAgency xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.messageErrorAgency + "</messageErrorAgency>");
        //    LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.status + "</status>");
        //    LarXML.Append("      <messageError xmlns='http://processSchema.eGastos/'>" + me.UltFlobotVariables.messageError + "</messageError>");
        //    LarXML.Append("    </UltFlobotVariables>");
        //}
        //else
        //{
        //    LarXML.Append("    <UltFlobotVariables xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <statusAgencyFlobot xmlns='http://processSchema.eGastos/'>0</statusAgencyFlobot>");
        //    LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>0</status>");
        //    LarXML.Append("    </UltFlobotVariables>");
        //}

        //if (me.UltHotel != null)
        //{
        //    foreach (eGastosEntity.Ultimus.UltHotel obj in me.UltHotel)
        //    {
        //        LarXML.Append("    <UltHotel xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <idHotel xmlns='http://processSchema.eGastos/'>" + obj.idHotel + "</idHotel>");
        //        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
        //        LarXML.Append("      <idRated xmlns='http://processSchema.eGastos/'>" + obj.idRated + "</idRated>");
        //        LarXML.Append("      <idConsecutive xmlns='http://processSchema.eGastos/'>" + obj.idConsecutive + "</idConsecutive>");
        //        LarXML.Append("      <idLegerAccount xmlns='http://processSchema.eGastos/'>" + obj.idLegerAccount + "</idLegerAccount>");
        //        LarXML.Append("      <nameLegerAccount xmlns='http://processSchema.eGastos/'>" + obj.nameLegerAccount + "</nameLegerAccount>");
        //        LarXML.Append("      <country xmlns='http://processSchema.eGastos/'>" + obj.country + "</country>");
        //        LarXML.Append("      <city xmlns='http://processSchema.eGastos/'>" + obj.city + "</city>");
        //        LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
        //        LarXML.Append("      <checkInDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.checkInDate) + "</checkInDate>");
        //        LarXML.Append("      <checkoutDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.checkoutDate) + "</checkoutDate>");
        //        LarXML.Append("      <hotelName xmlns='http://processSchema.eGastos/'>" + obj.hotelName + "</hotelName>");
        //        LarXML.Append("      <reservation xmlns='http://processSchema.eGastos/'>" + obj.reservation + "</reservation>");
        //        LarXML.Append("      <telephone xmlns='http://processSchema.eGastos/'>" + obj.telephone + "</telephone>");
        //        LarXML.Append("      <address xmlns='http://processSchema.eGastos/'>" + obj.address + "</address>");
        //        LarXML.Append("      <quotedRate xmlns='http://processSchema.eGastos/'>" + obj.quotedRate + "</quotedRate>");
        //        LarXML.Append("      <realRate xmlns='http://processSchema.eGastos/'>" + obj.realRate + "</realRate>");
        //        LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + obj.IVA + "</IVA>");
        //        LarXML.Append("      <hotelTax xmlns='http://processSchema.eGastos/'>" + obj.hotelTax + "</hotelTax>");
        //        LarXML.Append("      <otherTaxes xmlns='http://processSchema.eGastos/'>" + obj.otherTaxes + "</otherTaxes>");
        //        LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
        //        LarXML.Append("      <lineStatus xmlns='http://processSchema.eGastos/'>" + obj.lineStatus + "</lineStatus>");
        //        LarXML.Append("      <lineStatusName xmlns='http://processSchema.eGastos/'>" + obj.lineStatusName + "</lineStatusName>");
        //        LarXML.Append("    </UltHotel>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("    <UltHotel xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <idHotel xmlns='http://processSchema.eGastos/'>0</idHotel>");
        //    LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
        //    LarXML.Append("    <idRated xmlns='http://processSchema.eGastos/'>0</idRated>");
        //    LarXML.Append("    <idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive>");
        //    LarXML.Append("    <idLegerAccount xmlns='http://processSchema.eGastos/'>0</idLegerAccount>");
        //    LarXML.Append("    <checkInDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkInDate>");
        //    LarXML.Append("    <checkoutDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</checkoutDate>");
        //    LarXML.Append("    <quotedRate xmlns='http://processSchema.eGastos/'>0</quotedRate>");
        //    LarXML.Append("    <realRate xmlns='http://processSchema.eGastos/'>0</realRate>");
        //    LarXML.Append("    <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
        //    LarXML.Append("    <hotelTax xmlns='http://processSchema.eGastos/'>0</hotelTax>");
        //    LarXML.Append("    <otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes>");
        //    LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>false</status>");
        //    LarXML.Append("    <lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus>");
        //    LarXML.Append("    </UltHotel>");
        //}

        //if (me.UltItinerary != null)
        //{

        //    foreach (eGastosEntity.Ultimus.UltItinerary obj in me.UltItinerary)
        //    {
        //        LarXML.Append("    <UltItinerary xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <idItinerary xmlns='http://processSchema.eGastos/'>" + obj.idItinerary + "</idItinerary>");
        //        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
        //        LarXML.Append("      <idConsecutive xmlns='http://processSchema.eGastos/'>" + obj.idConsecutive + "</idConsecutive>");
        //        LarXML.Append("      <idLedgerAccount xmlns='http://processSchema.eGastos/'>" + obj.idLedgerAccount + "</idLedgerAccount>");
        //        LarXML.Append("      <nameLedgerAccount xmlns='http://processSchema.eGastos/'>" + obj.nameLedgerAccount + "</nameLedgerAccount>");
        //        LarXML.Append("      <departureHour xmlns='http://processSchema.eGastos/'>" + obj.departureHour + "</departureHour>");
        //        LarXML.Append("      <returnHour xmlns='http://processSchema.eGastos/'>" + obj.returnHour + "</returnHour>");
        //        LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
        //        LarXML.Append("      <travelType xmlns='http://processSchema.eGastos/'>" + obj.travelType + "</travelType>");
        //        LarXML.Append("      <nameTravelType xmlns='http://processSchema.eGastos/'>" + obj.nameTravelType + "</nameTravelType>");
        //        LarXML.Append("      <departureCountry xmlns='http://processSchema.eGastos/'>" + obj.departureCountry + "</departureCountry>");
        //        LarXML.Append("      <departureCity xmlns='http://processSchema.eGastos/'>" + obj.departureCity + "</departureCity>");
        //        LarXML.Append("      <arrivalCountry xmlns='http://processSchema.eGastos/'>" + obj.arrivalCountry + "</arrivalCountry>");
        //        LarXML.Append("      <arrivalCity xmlns='http://processSchema.eGastos/'>" + obj.arrivalCity + "</arrivalCity>");
        //        LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.departureDate) + "</departureDate>");
        //        LarXML.Append("      <arrivalDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.arrivalDate) + "</arrivalDate>");
        //        LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + obj.status.ToString().ToLower() + "</status>");
        //        LarXML.Append("    </UltItinerary>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("    <UltItinerary xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <idItinerary xmlns='http://processSchema.eGastos/'>0</idItinerary>");
        //    LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
        //    LarXML.Append("    <idConsecutive xmlns='http://processSchema.eGastos/'>0</idConsecutive>");
        //    LarXML.Append("    <idLedgerAccount xmlns='http://processSchema.eGastos/'>0</idLedgerAccount>");
        //    LarXML.Append("    <travelType xmlns='http://processSchema.eGastos/'>0</travelType>");
        //    LarXML.Append("    <departureDate xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
        //    LarXML.Append("    <arrivalDate xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
        //    LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>false</status>");
        //    LarXML.Append("    </UltItinerary>");
        //}

        //if (me.UltItineraryOptions != null)
        //{
        //    foreach (UltItineraryOptions obj in me.UltItineraryOptions)
        //    {
        //        LarXML.Append("    <UltItineraryOptions xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <idItineraryOption xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOption + "</idItineraryOption>");
        //        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
        //        LarXML.Append("      <idRate xmlns='http://processSchema.eGastos/'>" + obj.idRate + "</idRate>");
        //        LarXML.Append("      <quoteRate xmlns='http://processSchema.eGastos/'>" + obj.quoteRate + "</quoteRate>");
        //        LarXML.Append("      <observations xmlns='http://processSchema.eGastos/'>" + obj.observations + "</observations>");
        //        LarXML.Append("      <confirmed xmlns='http://processSchema.eGastos/'>" + obj.confirmed.ToString().ToLower() + "</confirmed>");
        //        LarXML.Append("      <lastDayPurchase xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.lastDayPurchase) + "</lastDayPurchase>");
        //        LarXML.Append("    </UltItineraryOptions>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("    <UltItineraryOptions xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption>");
        //    LarXML.Append("    <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
        //    LarXML.Append("    <idRate xmlns='http://processSchema.eGastos/'>0</idRate>");
        //    LarXML.Append("    <quoteRate xmlns='http://processSchema.eGastos/'>0</quoteRate>");
        //    LarXML.Append("    <confirmed xmlns='http://processSchema.eGastos/'>false</confirmed>");
        //    LarXML.Append("    <lastDayPurchase xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</lastDayPurchase>");
        //    LarXML.Append("    </UltItineraryOptions>");
        //}

        //if (me.UltItineraryOptionsDetail != null)
        //{
        //    foreach (UltItineraryOptionsDetail obj in me.UltItineraryOptionsDetail)
        //    {
        //        LarXML.Append("    <UltItineraryOptionsDetail xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOptionsDetail + "</idItineraryOptionsDetail>");
        //        LarXML.Append("      <idItineraryOption xmlns='http://processSchema.eGastos/'>" + obj.idItineraryOption + "</idItineraryOption>");
        //        LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + obj.idMissionOrder + "</idMissionOrder>");
        //        LarXML.Append("      <airlineFlight xmlns='http://processSchema.eGastos/'>" + obj.airlineFlight + "</airlineFlight>");
        //        LarXML.Append("      <departure xmlns='http://processSchema.eGastos/'>" + obj.departure + "</departure>");
        //        LarXML.Append("      <arrival xmlns='http://processSchema.eGastos/'>" + obj.arrival + "</arrival>");
        //        LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.departureDate) + "</departureDate>");
        //        LarXML.Append("      <arrivalDate xmlns='http://processSchema.eGastos/'>" + String.Format("{0:s}", obj.arrivalDate) + "</arrivalDate>");
        //        LarXML.Append("      <lapseTime xmlns='http://processSchema.eGastos/'>" + obj.lapseTime + "</lapseTime>");
        //        LarXML.Append("    </UltItineraryOptionsDetail>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("        <UltItineraryOptionsDetail xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("        <idItineraryOptionsDetail xmlns='http://processSchema.eGastos/'>0</idItineraryOptionsDetail>");
        //    LarXML.Append("        <idItineraryOption xmlns='http://processSchema.eGastos/'>0</idItineraryOption>");
        //    LarXML.Append("        <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
        //    LarXML.Append("        <departureDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</departureDate>");
        //    LarXML.Append("        <arrivalDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</arrivalDate>");
        //    LarXML.Append("        <lapseTime xmlns='http://processSchema.eGastos/'>0</lapseTime>");
        //    LarXML.Append("        </UltItineraryOptionsDetail>");
        //}

        //if (me.UltItineraryRate != null)
        //{
        //    LarXML.Append("    <UltItineraryRate xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <IdItineraryRate xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.IdItineraryRate + "</IdItineraryRate>");
        //    LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.idMissionOrder + "</idMissionOrder>");
        //    LarXML.Append("      <realRate xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.realRate + "</realRate>");
        //    LarXML.Append("      <IVA xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.IVA + "</IVA>");
        //    LarXML.Append("      <TUA xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.TUA + "</TUA>");
        //    LarXML.Append("      <otherTaxes xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.otherTaxes + "</otherTaxes>");
        //    LarXML.Append("      <lineStatus xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.lineStatus + "</lineStatus>");
        //    LarXML.Append("      <lineStatusName xmlns='http://processSchema.eGastos/'>" + me.UltItineraryRate.lineStatusName + "</lineStatusName>");
        //    LarXML.Append("    </UltItineraryRate>");
        //}
        //else
        //{
        //    LarXML.Append("        <UltItineraryRate xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("            <IdItineraryRate xmlns='http://processSchema.eGastos/'>0</IdItineraryRate>");
        //    LarXML.Append("            <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
        //    LarXML.Append("            <realRate xmlns='http://processSchema.eGastos/'>0</realRate>");
        //    LarXML.Append("            <IVA xmlns='http://processSchema.eGastos/'>0</IVA>");
        //    LarXML.Append("            <TUA xmlns='http://processSchema.eGastos/'>0</TUA>");
        //    LarXML.Append("            <otherTaxes xmlns='http://processSchema.eGastos/'>0</otherTaxes>");
        //    LarXML.Append("            <lineStatus xmlns='http://processSchema.eGastos/'>0</lineStatus>");
        //    LarXML.Append("        </UltItineraryRate>");
        //}

        //if (me.UltMissionOrder != null)
        //{
        //    LarXML.Append("    <UltMissionOrder xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idMissionOrder + "</idMissionOrder>");
        //    LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idRequest + "</idRequest>");
        //    LarXML.Append("      <idAgencyResponse xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idAgencyResponse + "</idAgencyResponse>");
        //    LarXML.Append("      <statusAgencyProcess xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.statusAgencyProcess + "</statusAgencyProcess>");
        //    LarXML.Append("      <statusAgencySend xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.statusAgencySend + "</statusAgencySend>");
        //    LarXML.Append("      <countAgencyWait xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.countAgencyWait + "</countAgencyWait>");
        //    LarXML.Append("      <idAgencyLog xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.idAgencyLog + "</idAgencyLog>");
        //    LarXML.Append("      <travelId xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.travelId + "</travelId>");
        //    LarXML.Append("      <travelName xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.travelName + "</travelName>");
        //    LarXML.Append("      <objective xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.objective + "</objective>");
        //    LarXML.Append("      <advance xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.advance + "</advance>");
        //    LarXML.Append("      <nationalCurrency xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.nationalCurrency + "</nationalCurrency>");
        //    LarXML.Append("      <advanceApply xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.advanceApply.ToString().ToLower() + "</advanceApply>");
        //    LarXML.Append("      <itinerary xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.itinerary.ToString().ToLower() + "</itinerary>");
        //    LarXML.Append("      <hotel xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.hotel.ToString().ToLower() + "</hotel>");
        //    if (!string.IsNullOrEmpty(me.UltMissionOrder.comment))
        //    {
        //        LarXML.Append("      <comment xmlns='http://processSchema.eGastos/'>" + me.UltMissionOrder.comment + "</comment>");
        //    }
        //    LarXML.Append("    </UltMissionOrder>");
        //}
        //else
        //{
        //    LarXML.Append("      <UltMissionOrder xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <idMissionOrder xmlns='http://processSchema.eGastos/'>0</idMissionOrder>");
        //    LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
        //    LarXML.Append("      <idAgencyResponse xmlns='http://processSchema.eGastos/'>0</idAgencyResponse>");
        //    LarXML.Append("      <statusAgencyProcess xmlns='http://processSchema.eGastos/'>0</statusAgencyProcess>");
        //    LarXML.Append("      <statusAgencySend xmlns='http://processSchema.eGastos/'>0</statusAgencySend>");
        //    LarXML.Append("      <countAgencyWait xmlns='http://processSchema.eGastos/'>0</countAgencyWait>");
        //    LarXML.Append("      <idAgencyLog xmlns='http://processSchema.eGastos/'>0</idAgencyLog>");
        //    LarXML.Append("      <advance xmlns='http://processSchema.eGastos/'>0</advance>");
        //    LarXML.Append("      <nationalCurrency xmlns='http://processSchema.eGastos/'>0</nationalCurrency>");
        //    LarXML.Append("      <advanceApply xmlns='http://processSchema.eGastos/'>false</advanceApply>");
        //    LarXML.Append("      <itinerary xmlns='http://processSchema.eGastos/'>false</itinerary>");
        //    LarXML.Append("      <hotel xmlns='http://processSchema.eGastos/'>false</hotel>");
        //    LarXML.Append("      </UltMissionOrder>");
        //}

        //if (me.UltPAClient != null)
        //{
        //    foreach (eGastosEntity.Ultimus.UltPAClient obj in me.UltPAClient)
        //    {
        //        LarXML.Append("    <UltPAClient xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>" + obj.idExpenseAccountDetail + "</idExpenseAccountDetail>");
        //        LarXML.Append("      <code xmlns='http://processSchema.eGastos/'>" + obj.code + "</code>");
        //        LarXML.Append("      <name xmlns='http://processSchema.eGastos/'>" + obj.name + "</name>");
        //        LarXML.Append("    </UltPAClient>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("    <UltPAClient xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <idExpenseAccountDetail xmlns='http://processSchema.eGastos/'>0</idExpenseAccountDetail>");
        //    LarXML.Append("    </UltPAClient>");
        //}

        //if (me.UltRequest != null)
        //{
        //    LarXML.Append("    <UltRequest xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + me.UltRequest.idRequest + "</idRequest>");
        //    LarXML.Append("      <requestDate xmlns='http://processSchema.eGastos/'>" + strHoraAtual + "</requestDate>");
        //    if (!string.IsNullOrEmpty(me.UltRequest.companyName))
        //    {
        //        LarXML.Append("      <companyName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.companyName + "</companyName>");
        //    }
        //    LarXML.Append("      <companyCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.companyCode + "</companyCode>");
        //    LarXML.Append("      <CeCoCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoCode + "</CeCoCode>");
        //    LarXML.Append("      <CeCoName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoName + "</CeCoName>");
        //    LarXML.Append("      <CeCoMiniCode xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoMiniCode + "</CeCoMiniCode>");
        //    LarXML.Append("      <CeCoMiniName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.CeCoMiniName + "</CeCoMiniName>");
        //    LarXML.Append("      <isMiniEvent xmlns='http://processSchema.eGastos/'>" + me.UltRequest.isMiniEvent.ToString().ToLower() + "</isMiniEvent>");
        //    if (!string.IsNullOrEmpty(me.UltRequest.arrival))
        //    {
        //        LarXML.Append("      <arrival xmlns='http://processSchema.eGastos/'>" + me.UltRequest.arrival + "</arrival>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.departureDate))
        //    {
        //        LarXML.Append("      <departureDate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.departureDate + "</departureDate>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.returnDate))
        //    {
        //        LarXML.Append("      <returnDate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.returnDate + "</returnDate>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.PEPElementId))
        //    {
        //        LarXML.Append("      <PEPElementId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PEPElementId + "</PEPElementId>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.PEPElementName))
        //    {
        //        LarXML.Append("      <PEPElementName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PEPElementName + "</PEPElementName>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.currencyId))
        //    {
        //        LarXML.Append("      <currencyId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.currencyId + "</currencyId>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.currencyName))
        //    {
        //        LarXML.Append("      <currencyName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.currencyName + "</currencyName>");
        //    }
        //    LarXML.Append("      <exchangeRate xmlns='http://processSchema.eGastos/'>" + me.UltRequest.exchangeRate + "</exchangeRate>");
        //    if (!string.IsNullOrEmpty(me.UltRequest.initiatorLogin))
        //    {
        //        LarXML.Append("      <initiatorLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequest.initiatorLogin + "</initiatorLogin>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.initiatorName))
        //    {
        //        LarXML.Append("      <initiatorName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.initiatorName + "</initiatorName>");
        //    }
        //    LarXML.Append("      <PAClientId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PAClientId + "</PAClientId>");
        //    LarXML.Append("      <PAClientName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.PAClientName + "</PAClientName>");
        //    if (!string.IsNullOrEmpty(me.UltRequest.responsibleLogin))
        //    {
        //        LarXML.Append("      <responsibleLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleLogin + "</responsibleLogin>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.responsibleName))
        //    {
        //        LarXML.Append("      <responsibleName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleName + "</responsibleName>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.responsibleEmployeeNum))
        //    {
        //        LarXML.Append("      <responsibleEmployeeNum xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleEmployeeNum + "</responsibleEmployeeNum>");
        //    }
        //    if (!string.IsNullOrEmpty(me.UltRequest.responsibleUserName))
        //    {
        //        LarXML.Append("      <responsibleUserName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsibleUserName + "</responsibleUserName>");
        //    }
        //    LarXML.Append("      <responsiblePayMethod xmlns='http://processSchema.eGastos/'>" + me.UltRequest.responsiblePayMethod + "</responsiblePayMethod>");
        //    LarXML.Append("      <pasteur xmlns='http://processSchema.eGastos/'>" + me.UltRequest.pasteur.ToString().ToLower() + "</pasteur>");
        //    LarXML.Append("      <areaId xmlns='http://processSchema.eGastos/'>" + me.UltRequest.areaId + "</areaId>");
        //    if (!string.IsNullOrEmpty(me.UltRequest.areaText))
        //    {
        //        LarXML.Append("      <areaText xmlns='http://processSchema.eGastos/'>" + me.UltRequest.areaText + "</areaText>");
        //    }
        //    LarXML.Append("      <ultimusNumber xmlns='http://processSchema.eGastos/'>" + me.UltRequest.ultimusNumber + "</ultimusNumber>");

        //    LarXML.Append("      <type xmlns='http://processSchema.eGastos/'>" + me.UltRequest.type + "</type>");
        //    LarXML.Append("      <typeName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.typeName + "</typeName>");
        //    LarXML.Append("      <status xmlns='http://processSchema.eGastos/'>" + me.UltRequest.status + "</status>");
        //    LarXML.Append("      <statusName xmlns='http://processSchema.eGastos/'>" + me.UltRequest.statusName + "</statusName>");
        //    LarXML.Append("      <salesForce xmlns='http://processSchema.eGastos/'>" + me.UltRequest.salesForce.ToString().ToLower() + "</salesForce>");
        //    LarXML.Append("    </UltRequest>");
        //}
        //else
        //{
        //    LarXML.Append("    <UltRequest xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
        //    LarXML.Append("    <requestDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requestDate>");
        //    LarXML.Append("    <companyCode xmlns='http://processSchema.eGastos/'>0</companyCode>");
        //    LarXML.Append("    <CeCoCode xmlns='http://processSchema.eGastos/'>0</CeCoCode>");
        //    LarXML.Append("    <CeCoMiniCode xmlns='http://processSchema.eGastos/'>0</CeCoMiniCode>");
        //    LarXML.Append("    <isMiniEvent xmlns='http://processSchema.eGastos/'>false</isMiniEvent>");
        //    LarXML.Append("    <exchangeRate xmlns='http://processSchema.eGastos/'>0</exchangeRate>");
        //    LarXML.Append("    <pasteur xmlns='http://processSchema.eGastos/'>false</pasteur>");
        //    LarXML.Append("    <areaId xmlns='http://processSchema.eGastos/'>0</areaId>");
        //    LarXML.Append("    <ultimusNumber xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
        //    LarXML.Append("    <type xmlns='http://processSchema.eGastos/'>0</type>");
        //    LarXML.Append("    <status xmlns='http://processSchema.eGastos/'>0</status>");
        //    LarXML.Append("    <salesForce xsi:nil='true' xmlns='http://processSchema.eGastos/' />");
        //    LarXML.Append("    </UltRequest>");
        //}
        //if (me.UltRequester != null)
        //{
        //    LarXML.Append("    <UltRequester xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("      <requesterName xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterName + "</requesterName>");
        //    LarXML.Append("      <requesterLogin xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterLogin + "</requesterLogin>");
        //    LarXML.Append("      <requesterEmail xmlns='http://processSchema.eGastos/'>" + me.UltRequester.requesterEmail + "</requesterEmail>");
        //    LarXML.Append("      <requesterDate xmlns='http://processSchema.eGastos/'>" + ToXMLDateFormat(me.UltRequester.requesterDate) + "</requesterDate>");
        //    LarXML.Append("    </UltRequester>");
        //}
        //else
        //{
        //    LarXML.Append("    <UltRequester xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <requesterDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</requesterDate>");
        //    LarXML.Append("    </UltRequester>");
        //}

        //if (me.UltResponsible != null)
        //{
        //    LarXML.Append("    <UltResponsible xmlns='" + ProcessVersionNumber + "' >");
        //    LarXML.Append("      <responsibleName xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleName + "</responsibleName>");
        //    LarXML.Append("      <responsibleLogin xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleLogin + "</responsibleLogin>");
        //    LarXML.Append("      <responsibleEmail xmlns='http://processSchema.eGastos/'>" + me.UltResponsible.responsibleEmail + "</responsibleEmail>");
        //    LarXML.Append("    </UltResponsible>");
        //}
        //else
        //{
        //    LarXML.Append("    <UltResponsible xmlns='" + ProcessVersionNumber + "' />");
        //}

        //if (me.UltSAPResponse != null)
        //{
        //    foreach (UltSAPResponse obj in me.UltSAPResponse)
        //    {
        //        LarXML.Append("    <UltSAPResponse xmlns='" + ProcessVersionNumber + "'>");
        //        LarXML.Append("      <idResponse xmlns='http://processSchema.eGastos/'>" + obj.idResponse + "</idResponse>");
        //        LarXML.Append("      <idRequest xmlns='http://processSchema.eGastos/'>" + obj.idRequest + "</idRequest>");
        //        LarXML.Append("      <docNumber xmlns='http://processSchema.eGastos/'>" + obj.docNumber + "</docNumber>");
        //        LarXML.Append("      <company xmlns='http://processSchema.eGastos/'>" + obj.company + "</company>");
        //        LarXML.Append("      <year xmlns='http://processSchema.eGastos/'>" + obj.year + "</year>");
        //        LarXML.Append("      <type xmlns='http://processSchema.eGastos/'>" + obj.type + "</type>");
        //        LarXML.Append("    </UltSAPResponse>");
        //    }
        //}
        //else
        //{
        //    LarXML.Append("    <UltSAPResponse xmlns='" + ProcessVersionNumber + "'>");
        //    LarXML.Append("    <idResponse xmlns='http://processSchema.eGastos/'>0</idResponse>");
        //    LarXML.Append("    <idRequest xmlns='http://processSchema.eGastos/'>0</idRequest>");
        //    LarXML.Append("    <docNumber xmlns='http://processSchema.eGastos/'>0</docNumber>");
        //    LarXML.Append("    <company xmlns='http://processSchema.eGastos/'>0</company>");
        //    LarXML.Append("    <year xmlns='http://processSchema.eGastos/'>0</year>");
        //    LarXML.Append("    </UltSAPResponse>");
        //}

        //LarXML.Append("        <Cancel xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <URLAttach xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <DaysOutOfCountry xmlns='" + ProcessVersionNumber + "'>0</DaysOutOfCountry>");
        //LarXML.Append("        <WSAgencyName xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <WSAgencyURL xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <WSChangeRequestName xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <WSChangeRequestURL xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <EmailAgency xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <Travel_Nacional xmlns='" + ProcessVersionNumber + "'>false</Travel_Nacional>");
        //LarXML.Append("        <Travel_Internacional xmlns='" + ProcessVersionNumber + "'>false</Travel_Internacional>");
        //LarXML.Append("        <Travel_Domestic xmlns='" + ProcessVersionNumber + "'>false</Travel_Domestic>");
        //LarXML.Append("        <UltGlobal xmlns='" + ProcessVersionNumber + "'>");
        //LarXML.Append("          <idSession xmlns='http://processSchema.eGastos/'>0</idSession>");
        //LarXML.Append("        </UltGlobal>");
        //LarXML.Append("        <ExpenseAccountTotal xmlns='" + ProcessVersionNumber + "'>");
        //LarXML.Append("          <WithoutIVA>0</WithoutIVA>");
        //LarXML.Append("          <IVA>0</IVA>");
        //LarXML.Append("          <Amount>0</Amount>");
        //LarXML.Append("        </ExpenseAccountTotal>");
        //LarXML.Append("        <delayExpenseAccount xsi:nil='true' xmlns='" + ProcessVersionNumber + "' />");
        //LarXML.Append("        <completionExpenseAccount xmlns='" + ProcessVersionNumber + "'>5</completionExpenseAccount>");
        //LarXML.Append("      </Global>");
        //LarXML.Append("      <StepSchemaUltApprovalHistory>");
        //LarXML.Append("        <approveDate xmlns='http://processSchema.eGastos/'>0001-01-01T00:00:00</approveDate>");
        //LarXML.Append("      </StepSchemaUltApprovalHistory>");
        //LarXML.Append("      <SYS_PROCESSATTACHMENTS />");
        //LarXML.Append("    </TaskData>");
        ////-------------
        //strxml1 = LarXML.ToString();
        //strError = "";
    }
    }
}
