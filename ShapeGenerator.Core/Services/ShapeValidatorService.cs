using System;
using System.Collections.Generic;
using System.Linq;
using ShapeGenerator.Core.Models;
using ShapeGenerator.Core.Helpers;

namespace ShapeGenerator.Core
{
    public interface IShapeValidator {
        ShapeModel ValidateData(string userInput);
    }

    public class ShapeValidatorService : IShapeValidator
    {
        public enum ShapeAttributeEnum
        {
            shape,
            measurement,
            amount
        }

        public enum MeasurementEnum
        {
            radius,
            height,
            width,
            sidelength
        }

        public const double maxAmount = 1000;

        private List<string> requiredWordsList = new List<string> { "Draw", "a/an", ShapeAttributeEnum.shape.ToString(), "with", "a",
            ShapeAttributeEnum.measurement.ToString(), "of", ShapeAttributeEnum.amount.ToString()};

        private List<string> optionalWordsList = new List<string> { "a/an", ShapeAttributeEnum.measurement.ToString(), "of",
            ShapeAttributeEnum.amount.ToString()};

        /// <summary>
        /// validate user data and return details of Shape if validation is valid
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public ShapeModel ValidateData (string userInput)
        {
            ShapeModel model = new ShapeModel();
            try
            {
                bool isValidRequiredStatement, isValidOptionalStatement = true;
                if (userInput.Split(" ").Length >= 8)
                {
                    var splitStatement = userInput.Split("and");
                    isValidRequiredStatement = ValidateRequiredStatement(splitStatement[0].Trim());

                    if (splitStatement.Length > 1)
                    {
                        for (int j = 1; j < splitStatement.Length; j++)
                        {
                            isValidOptionalStatement = ValidateAdditionalStatement(splitStatement[j].Trim());
                            if (!isValidOptionalStatement)
                                break;
                        }
                    }

                    if (isValidRequiredStatement && isValidOptionalStatement)
                        model = GetShapeDetails(splitStatement);
                    else
                        model.ErrorMessage = ErrorConstants.inValidInput;
                }
                else
                    model.ErrorMessage = ErrorConstants.inValidInput;
            }
            catch (Exception ex)
            {
                model.ErrorMessage = $"{ErrorConstants.UnhandledError} {ex.Message}";
            }

            return model;
        }

        #region Private Functions

        /// <summary>
        /// Validate the Required statemnt for Shape generation entered by user.
        /// </summary>
        /// <param name="requiredStatement"></param>
        /// <returns></returns>
        private bool ValidateRequiredStatement(string requiredStatement)
        {
            bool isvalid = true;
            var splitRequiredStatement = requiredStatement.Split(" ");
            if (splitRequiredStatement.Length >= 8 && splitRequiredStatement.Length <= 10)
            {
                int userStatementCount = 0;
                for (int i = 0; i < requiredWordsList.Count; i++)
                {
                    if (i == 2)
                    {
                        //validate shape
                        string shapeType = ShapeConstants.SupportedShapesList.FirstOrDefault(x => requiredStatement.Contains(x.ToLower()));
                        if (shapeType == null)
                            isvalid = false;
                        else
                            userStatementCount = userStatementCount + shapeType.Split(" ").Length - 1;
                    }
                    else if (i == 5)
                    {
                        //validate measurement
                        string measurement = ShapeConstants.MeasurementList.FirstOrDefault(x => requiredStatement.Contains(x.ToLower()));
                        if (measurement == null)
                            isvalid = false;
                        else
                            userStatementCount = userStatementCount + measurement.Split(" ").Length - 1;
                    }
                    else if (requiredWordsList[i] == ShapeAttributeEnum.amount.ToString())
                    {
                        //Validate Amount
                        int amount = 0;
                        var isNumber = int.TryParse(splitRequiredStatement[userStatementCount], out amount);
                        if (!isNumber || amount <= 0 || amount > maxAmount)
                            isvalid = false;
                    }
                    else if (!requiredWordsList[i].ToLower().Split("/").Contains(splitRequiredStatement[userStatementCount].ToLower()))
                        isvalid = false;

                    userStatementCount++;
                }
            }
            else
            {
                isvalid = false;
            }

            return isvalid;
        }

        /// <summary>
        /// Validate additional information that the user has entered
        /// </summary>
        /// <param name="optionalStatement"></param>
        /// <returns></returns>
        private bool ValidateAdditionalStatement(string optionalStatement)
        {
            bool isvalid = true;
            var splitOptionalStatement = optionalStatement.Split(" ");
            if (splitOptionalStatement.Length >= 4 && splitOptionalStatement.Length <= 5)
            {
                int userStatementCount = 0;
                for (int i = 0; i < optionalWordsList.Count; i++)
                {
                    if (optionalWordsList[i] == ShapeAttributeEnum.measurement.ToString())
                    {
                        //validate Measurement
                        string measurement = splitOptionalStatement[userStatementCount];
                        if (splitOptionalStatement.Length == 5)
                        {
                            userStatementCount++;
                            measurement = $"{measurement} {splitOptionalStatement[userStatementCount]}";
                        }
                        if (!ShapeConstants.MeasurementList.Contains(measurement))
                            isvalid = false;
                    }
                    else if (optionalWordsList[i] == ShapeAttributeEnum.amount.ToString())
                    {
                        //Validate Amount
                        int amount = 0;
                        var isNumber = int.TryParse(splitOptionalStatement[userStatementCount], out amount);
                        if (!isNumber || amount <= 0 || amount > maxAmount)
                            isvalid = false;
                    }
                    else if (!optionalWordsList[i].ToLower().Split("/").Contains(splitOptionalStatement[userStatementCount].ToLower()))
                        isvalid = false;

                    userStatementCount++;
                }
            }
            else
            {
                isvalid = false;
            }

            return isvalid;
        }

        /// <summary>
        /// Get Shape Details after the validation is done.
        /// </summary>
        /// <param name="userStatement"></param>
        /// <returns></returns>
        private ShapeModel GetShapeDetails(string[] userStatement)
        {
            var model = new ShapeModel
            {
                ShapeType = ShapeConstants.SupportedShapesList.First(x => userStatement[0].ToLower().Contains(x.ToLower())),
                SideLength = new List<double>()
            };

            foreach (string strStatement in userStatement)
            {
                var measurement = ShapeConstants.MeasurementList.First(x => strStatement.Trim().ToLower().Contains(x.ToLower()));
                var amount = strStatement.Trim().Split(" ").Last();
                AssignValue(model, measurement, amount);
            }

            ValidateShapeSpecs(model);
            return model;
        }

        /// <summary>
        /// Assigning values to the Shape Model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="measurement"></param>
        /// <param name="amount"></param>
        private void AssignValue(ShapeModel model, string measurement, string amount)
        {
            if (measurement.Replace(" ", "") == MeasurementEnum.radius.ToString())
                model.Radius = double.Parse(amount);
            if (measurement.Replace(" ", "") == MeasurementEnum.height.ToString())
                model.Height = double.Parse(amount);
            if (measurement.Replace(" ", "") == MeasurementEnum.width.ToString())
                model.Width = double.Parse(amount);
            if (measurement.Replace(" ", "") == MeasurementEnum.sidelength.ToString())
                model.SideLength.Add(double.Parse(amount));
        }

        /// <summary>
        /// Validate if the specs enterd atches the requirement of the shapes.
        /// </summary>
        /// <param name="model"></param>
        private void ValidateShapeSpecs(ShapeModel model)
        {
            bool isValidShapeSpec = true;
            switch (model.ShapeType)
            {
                case ShapeConstants.ShapeTypes.ScaleneTriangle:
                    if (model.Height != 0 || model.Width != 0 || model.Radius != 0 || model.SideLength?.Count != 3)
                        isValidShapeSpec = false;
                    else
                    {
                        ///Validate sides of Scalene Triangle
                        double a = model.SideLength[0];
                        double b = model.SideLength[1];
                        double c = model.SideLength[2];

                        if (a + b <= c || a + c <= b || b + c <= a)
                            isValidShapeSpec = false;
                    }
                    break;
                case ShapeConstants.ShapeTypes.Parallelogram:
                    if (model.Height != 0 || model.Width != 0 || model.Radius != 0 || model.SideLength?.Count != 2)
                        isValidShapeSpec = false;
                    break;
                case ShapeConstants.ShapeTypes.Circle:
                    if (model.Height != 0 || model.Width != 0 || model.Radius <= 0 || model.SideLength?.Count > 0)
                        isValidShapeSpec = false;
                    break;
                case ShapeConstants.ShapeTypes.IsoscelesTriangle:
                case ShapeConstants.ShapeTypes.Rectangle:
                case ShapeConstants.ShapeTypes.Oval:
                    if (model.Height <= 0 || model.Width <= 0 || model.Radius != 0 || model.SideLength?.Count > 0)
                        isValidShapeSpec = false;
                    break;
                case ShapeConstants.ShapeTypes.EquilateralTriangle:
                case ShapeConstants.ShapeTypes.Square:
                case ShapeConstants.ShapeTypes.Pentagon:
                case ShapeConstants.ShapeTypes.Hexagon:
                case ShapeConstants.ShapeTypes.Heptagon:
                case ShapeConstants.ShapeTypes.Octagon:
                    if (model.Height != 0 || model.Width != 0 || model.Radius != 0 || model.SideLength?.Count != 1)
                        isValidShapeSpec = false;
                    break;
            }

            if (!isValidShapeSpec)
                model.ErrorMessage = string.Format(ErrorConstants.invalidSpecs, model.ShapeType);
        }

        #endregion

    }
}
