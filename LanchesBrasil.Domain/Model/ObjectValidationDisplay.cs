namespace LanchesBrasil.Commons.Model
{
    public class ObjectValidationDisplay
    {
        public string MessageDisplay { get; set; } = string.Empty;
        public ValidationDisplayEnum ValidationDisplayEnum { get; set; }

        public ObjectValidationDisplay()
        {
        }

        public ObjectValidationDisplay(string MessageDisplay, ValidationDisplayEnum ValidationDisplayEnum)
        {
            this.MessageDisplay = MessageDisplay;
            this.ValidationDisplayEnum = ValidationDisplayEnum;
        }

        public ObjectValidationDisplay(ResultResponse ResultResponse)
        {
            MessageDisplay = ResultResponse.Message ?? "Ocorreu um erro na operação.";
            ValidationDisplayEnum = ResultResponse.Success ? ValidationDisplayEnum.SUCCESS : ValidationDisplayEnum.ERROR;
        }
    }
}
