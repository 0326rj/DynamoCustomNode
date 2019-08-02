#region Reference
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;

using DynamoServices;

using Autodesk.Revit.DB;

using RevitServices.Persistence;
using RevitServices.Transactions;

using Revit.Elements;
using Revit.GeometryConversion;
#endregion


namespace KIM_LIBRARY_FOLDER
{
  // 2019. 7. 16

  public class PracticeSample
  {
    private PracticeSample()
    {
    }

    // Revit.Elements means wrapped Revit Element in Dynamo
    // Autodesk.Revit.DB means Unwrapped Revit Element in Dynamo


    // Wrapped
    public static Revit.Elements.Category GetCategory( Revit.Elements.Element inRevitElement )
    {
      var wrappedCategory = inRevitElement.GetCategory;

      return wrappedCategory;
    }

  
    public static Autodesk.Revit.DB.ElementId GetID(Revit.Elements.Element inRevitElement )
    {
      // Autodesk.Revit.DB.Element UnwrappedElement = inRevitElement.InternalElement;
      // Autodesk.Revit.DB.ElementId UnwrappedElementID = UnwrappedElement.Id;

      // Unwrap
      var UnwrappedElement = inRevitElement.InternalElement;
      var UnwrappedElementID = UnwrappedElement.Id;

      return UnwrappedElementID;
    }


    #region Built-In Parameter 취득
    
    public static Double GetInstanceParameter( Revit.Elements.Element inRevitElement )
    {
      // Unwrap
      var unwrappedElement = inRevitElement.InternalElement;
      var unwrappedElementParamValue = unwrappedElement.
        get_Parameter( BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION ).
        AsDouble();

      return unwrappedElementParamValue;
    }
    #endregion


    #region Built-In Parameter 설정/변경

    public static Revit.Elements.Element SetInstanceParameter(
      Revit.Elements.Element inRevitElement,
      Double inValue )
    {
      // Unwrap
      var UnwrappedElement = inRevitElement.InternalElement;

      // Start Transaction because changing the REVIT DATABASE
      // Get current Document (standard stuff)

      var doc = DocumentManager.Instance.CurrentDBDocument;
      TransactionManager.Instance.EnsureInTransaction( doc );

      UnwrappedElement.
        get_Parameter( BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION ).
        Set( inValue );

      // End Transaction because changing the Revit DATABASE
      TransactionManager.Instance.TransactionTaskDone();

      return inRevitElement;
    }
    #endregion


    #region 특정 레빗 데이터 생성/변경 => Transaction to Revit => Dynamo wrap 

    public static Revit.Elements.Element CreateZTSomthing( double inElevation )
    {
      // Get Current Document (standard stuff) +
      // Start Transaction because changing the Revit DATABASE

      var doc = DocumentManager.Instance.CurrentDBDocument;
      TransactionManager.Instance.EnsureInTransaction( doc );

      // End Transaction because changing the Revit DATABASE
      var newLevel = Autodesk.Revit.DB.Level.Create( doc, inElevation );

      TransactionManager.Instance.TransactionTaskDone();

      // Wrapit! since Revit element generated in Code and sent to Dynamo
      var wrappedLevel = newLevel.ToDSType( false );

      return wrappedLevel;
    }
    #endregion


    #region MyRegion
    public static Autodesk.DesignScript.Geometry.Point DszTPoint(
      double inX = 1,
      double inY = 1,
      double inZ = 1 )
    {
      // Autodesk.DesignScript.Geometry.Point is a Dynamo point
      var dynampPt = Autodesk.DesignScript.Geometry.Point.ByCoordinates( inX, inY, inZ );

      return dynampPt;
    } 
    #endregion

    

  }
}
