using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ploosqva.WebUtils.Controls
{
    /// <summary>
    /// Control which allows thorough validation of asp.net web controls without the need
    /// of using validation controls and displays itself as a summary. 
    /// It has been created to allow easy validation of Ajax controls 
    /// </summary>
    public class ValidationControl : CompositeControl
    {
        private Literal litValidationSummary;
        private readonly HtmlGenericControl errorsList = new HtmlGenericControl("ul");
        private bool isValid = true;
        private Color foreColor = Color.Red;
        /// <summary>
        /// List of control already validated in this request. It is needed to avoid controls not
        /// being highlighted if numerous checks are performed on that control and the
        /// last check succeeds
        /// </summary>
        private readonly List<string> idsValidated = new List<string>();

        /// <summary>
        /// Validity check method
        /// </summary>
        /// <returns>true if input is valid</returns>
        public delegate bool ValidationMethod();

        #region Properties
        /// <summary>
        /// Indicates wheather all controls have passed validation. 
        /// This can be used instead of Page#IsValid to control flow
        /// </summary>
        public bool IsValid
        {
            get
            {
                return isValid;
            }
        }
        /// <summary>
        /// Validation summary header, displayed above errors list
        /// </summary>
        public string HeaderText
        {
            get
            {
                if (ViewState["HeaderText"] == null)
                    HeaderText = "";

                return (string)ViewState["HeaderText"];
            }
            set
            {
                ViewState["HeaderText"] = value;
            }
        }
        public override bool Visible
        {
            get
            {
                return !isValid;
            }
        }
        #endregion

        #region Override methods
        protected override void CreateChildControls()
        {
            //Todo: Użwać ForeColor

            Controls.Clear();

            litValidationSummary = new Literal { Text = HeaderText };
            Controls.Add(litValidationSummary);

            Controls.Add(new LiteralControl("<br/>"));
            Controls.Add(errorsList);
        }
        #endregion

        #region Core validation functionality
        /// <summary>
        /// Runs check and shows error message if it fails
        /// </summary>
        /// <param name="validatedControl">controls validated</param>
        /// <param name="errorText">text shown if test fails</param>
        /// <param name="method">delegate to validation method</param>
        private void Validate(WebControl validatedControl, string errorText, ValidationMethod method)
        {
            EnsureChildControls();

            if (!method.Invoke())
            {
                ShowErrorMessage(errorsList, validatedControl, errorText);

                if (!idsValidated.Contains(validatedControl.ID))
                    idsValidated.Add(validatedControl.ID);

                isValid = false;
                return;
            }

            if (idsValidated.Contains(validatedControl.ID))
                return;

            validatedControl.CssClass = "";
            idsValidated.Add(validatedControl.ID);
        }

        /// <summary>
        /// Adds the error message to summary and highlights the control
        /// which failed it's check
        /// </summary>
        /// <param name="errorsPlaceholder">control which houses error messages</param>
        /// <param name="validatedControl">control which fails a check</param>
        /// <param name="errorText">error message</param>
        private static void ShowErrorMessage(Control errorsPlaceholder,
            WebControl validatedControl, string errorText)
        {
            errorsPlaceholder.Controls.Add(new Literal
            {
                // ToDo: change this to templated control
                Text = string.Format("<li>{0}</li>", errorText)
            });

            // ToDo: allow custom styling
            validatedControl.CssClass = "error";
        }
        #endregion

        #region Validation of required fields
        /// <summary>
        /// Check if TextBox isn't empty
        /// </summary>
        /// <param name="controlToValidate">TextBox control</param>
        /// <param name="errorText">error message</param>
        public void ValidateRequired(TextBox controlToValidate, string errorText)
        {
            Validate(controlToValidate, errorText, () => !string.IsNullOrEmpty(controlToValidate.Text));
        }
        /// <summary>
        /// Check if DropDownList has an option selected. This assumes that if the first item 
        /// is selected, nput is not valid
        /// </summary>
        /// <param name="controlToValidate">TextBox control</param>
        /// <param name="errorText">error message</param>
        public void ValidateRequired(DropDownList controlToValidate, string errorText)
        {
            Validate(controlToValidate, errorText, () => controlToValidate.SelectedIndex != 0);
        }
        /// <summary>
        /// Check if a ListControl (eg. CheckBoxList or RadioButtonList) has at least one
        /// option selected
        /// </summary>
        /// <param name="controlToValidate">ListControl control</param>
        /// <param name="errorText">error message</param>
        public void ValidateRequired(ListControl controlToValidate, string errorText)
        {
            Validate(controlToValidate, errorText, () => controlToValidate.SelectedIndex != -1);
        }
        /// <summary>
        /// Check if CheckBox is checked
        /// </summary>
        /// <param name="controlToValidate">CheckBox control</param>
        /// <param name="errorText">error message</param>
        public void ValidateRequired(CheckBox controlToValidate, string errorText)
        {
            Validate(controlToValidate, errorText, () => controlToValidate.Checked);
        }
        #endregion

        #region Validation through comaprison
        /// <summary>
        /// Checks a TextBox's intput data type ora the ralation between values from two textboxes. 
        /// If it fails, <paramref name="controlToValidate"/> will be highlighted
        /// </summary>
        /// <param name="controlToValidate">First TextBox</param>
        /// <param name="controlToCompare">Second TextBox. If performing data type check, this can be null</param>
        /// <param name="compareOperator">Type of check performed</param>
        /// <param name="dataType">Assumed data type input</param>
        /// <param name="errorText">error message</param>
        public void ValidateCompare(TextBox controlToValidate, TextBox controlToCompare,
            ValidationCompareOperator compareOperator, ValidationDataType dataType, string errorText)
        {
            if (compareOperator == ValidationCompareOperator.DataTypeCheck)
            {
                Validate(controlToValidate,
                    errorText, () => CheckDataType(controlToValidate, dataType));
                return;
            }

            switch (dataType)
            {
                case ValidationDataType.String:
                    Validate(controlToValidate, errorText,
                        () => CompareStrings(controlToValidate.Text, controlToCompare.Text, compareOperator));
                    break;
                case ValidationDataType.Integer:
                    Validate(controlToValidate, errorText,
                        () => CompareIntegers(controlToValidate.Text, controlToCompare.Text, compareOperator));
                    break;
                case ValidationDataType.Currency:
                    Validate(controlToValidate, errorText,
                        () => CompareCurrencies(controlToValidate.Text, controlToCompare.Text, compareOperator));
                    break;
                case ValidationDataType.Double:
                    Validate(controlToValidate, errorText,
                        () => CompareDoubles(controlToValidate.Text, controlToCompare.Text, compareOperator));
                    break;
                case ValidationDataType.Date:
                    Validate(controlToValidate, errorText,
                        () => CompareDates(controlToValidate.Text, controlToCompare.Text, compareOperator));
                    break;
            }
        }

        private static bool CheckDataType(TextBox controlToValidate, ValidationDataType dataType)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(controlToValidate.Text))
                switch (dataType)
                {
                    case ValidationDataType.String:
                        break;
                    case ValidationDataType.Integer:
                        int i;
                        result = Int32.TryParse(controlToValidate.Text, out i);
                        break;
                    case ValidationDataType.Currency:
                        throw new NotImplementedException("Currency not yet implemented");
                        break;
                    case ValidationDataType.Double:
                        double f;
                        result = Double.TryParse(controlToValidate.Text, out f);
                        break;
                    case ValidationDataType.Date:
                        DateTime d;
                        result = DateTime.TryParse(controlToValidate.Text, out d);
                        break;
                }

            return result;
        }

        private static bool CompareStrings(string left, string right, ValidationCompareOperator compareOperator)
        {
            bool result = true;

            switch (compareOperator)
            {
                case ValidationCompareOperator.Equal:
                    result = left.Equals(right);
                    break;
                case ValidationCompareOperator.NotEqual:
                    result = !left.Equals(right);
                    break;
                case ValidationCompareOperator.GreaterThan:
                    result = String.Compare(left, right) > 0;
                    break;
                case ValidationCompareOperator.GreaterThanEqual:
                    result = String.Compare(left, right) >= 0;
                    break;
                case ValidationCompareOperator.LessThan:
                    result = String.Compare(left, right) < 0;
                    break;
                case ValidationCompareOperator.LessThanEqual:
                    result = String.Compare(left, right) <= 0;
                    break;
            }

            return result;
        }

        private static bool CompareIntegers(string left, string right, ValidationCompareOperator compareOperator)
        {
            bool result = true;
            int leftInt, rightInt;

            if (!Int32.TryParse(left, out leftInt) || !Int32.TryParse(right, out rightInt))
                return false;

            switch (compareOperator)
            {
                case ValidationCompareOperator.Equal:
                    result = leftInt == rightInt;
                    break;
                case ValidationCompareOperator.NotEqual:
                    result = leftInt != rightInt;
                    break;
                case ValidationCompareOperator.GreaterThan:
                    result = leftInt > rightInt;
                    break;
                case ValidationCompareOperator.GreaterThanEqual:
                    result = leftInt >= rightInt;
                    break;
                case ValidationCompareOperator.LessThan:
                    result = leftInt < rightInt;
                    break;
                case ValidationCompareOperator.LessThanEqual:
                    result = leftInt <= rightInt;
                    break;
            }

            return result;
        }

        private static bool CompareDoubles(string left, string right, ValidationCompareOperator compareOperator)
        {
            bool result = true;
            double leftDouble, rightDouble;

            if (!Double.TryParse(left, out leftDouble) || !Double.TryParse(right, out rightDouble))
                return false;

            switch (compareOperator)
            {
                case ValidationCompareOperator.Equal:
                    result = leftDouble == rightDouble;
                    break;
                case ValidationCompareOperator.NotEqual:
                    result = leftDouble != rightDouble;
                    break;
                case ValidationCompareOperator.GreaterThan:
                    result = leftDouble > rightDouble;
                    break;
                case ValidationCompareOperator.GreaterThanEqual:
                    result = leftDouble >= rightDouble;
                    break;
                case ValidationCompareOperator.LessThan:
                    result = leftDouble < rightDouble;
                    break;
                case ValidationCompareOperator.LessThanEqual:
                    result = leftDouble <= rightDouble;
                    break;
            }

            return result;
        }

        private static bool CompareDates(string left, string right, ValidationCompareOperator compareOperator)
        {
            bool result = true;
            DateTime leftDate, rightDate;

            if (!DateTime.TryParse(left, out leftDate) || !DateTime.TryParse(right, out rightDate))
                return false;

            switch (compareOperator)
            {
                case ValidationCompareOperator.Equal:
                    result = leftDate == rightDate;
                    break;
                case ValidationCompareOperator.NotEqual:
                    result = leftDate != rightDate;
                    break;
                case ValidationCompareOperator.GreaterThan:
                    result = leftDate > rightDate;
                    break;
                case ValidationCompareOperator.GreaterThanEqual:
                    result = leftDate >= rightDate;
                    break;
                case ValidationCompareOperator.LessThan:
                    result = leftDate < rightDate;
                    break;
                case ValidationCompareOperator.LessThanEqual:
                    result = leftDate <= rightDate;
                    break;
            }

            return result;
        }

        private static bool CompareCurrencies(string left, string right, ValidationCompareOperator compareOperator)
        {
            throw new NotImplementedException("Currency not yet implemented");
        }
        #endregion

        #region Regular expression validation
        /// <summary>
        /// Check wheatrher TextBox's input matches a regular expression supplied
        /// </summary>
        /// <param name="controlToValidate">TextBox checked</param>
        /// <param name="regularExpression">Pattern to test against</param>
        /// <param name="errorMessage">error message</param>
        public void ValidateRegularExpression(TextBox controlToValidate, string regularExpression, string errorMessage)
        {
            Regex regex = new Regex(regularExpression);

            Validate(controlToValidate, errorMessage,
                () => string.IsNullOrEmpty(controlToValidate.Text) || regex.IsMatch(controlToValidate.Text));
        }
        #endregion

        #region Custom validation
        /// <summary>
        /// Allows to run a custom test
        /// </summary>
        /// <param name="control">Control to validate</param>
        /// <param name="errorText">error message</param>
        /// <param name="customValidationMethod">delegate to check method</param>
        public void CustomValidate(WebControl control, string errorText,
            ValidationMethod customValidationMethod)
        {
            Validate(control, errorText, customValidationMethod);
        }
        #endregion

        /// <summary>
        /// Sample regular expressions for use with regular expression validation method
        /// </summary>
        public class RegularExpressions
        {
            /// <summary>
            /// Standard email address
            /// </summary>
            public static string WebEmailAddress = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        }
    }
}
