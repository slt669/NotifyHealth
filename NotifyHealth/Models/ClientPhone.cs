namespace NotifyHealth.Models
{
    public class ClientPhone
    {
        public string Number { get; set; }
        public string PhoneCarrier { get; set; }
        public string MessageAddress { get; set; }

        public string Wireless { get; set; }
        public int PStatusId { get; set; }

        public string Status { get; set; }
        public int ParticipationId { get; set; }

        public string Warning { get; set; }

        //$("#PhoneCarrier").val(data["CarrierName"]);
        //        $("#MessageAddress").val(data["Address"]);
        //        $("#ParticipationId").val(data["ParticipationId"]);
        //        $("#PStatusId").val(data["ParsedStatus"]);
    }
}