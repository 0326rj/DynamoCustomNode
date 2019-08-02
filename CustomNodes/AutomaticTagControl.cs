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

namespace KIM_ZEROTOUCH_LIBRARY
{
  public class AutomaticTagControl
  {
    private AutomaticTagControl() { }

    const string _familyName = "DynamoControlled_slab1";
    const string _typeName1 = "set1";
    const string _typeName2 = "set2";
    const string _typeName3 = "set3";

    const string _param_name1 = "床スラブ_CON天端レベル";
    const string _param_name2 = "床スラブ_構造体天端レベル";
    const string _param_name3 = "床スラブ_部分ふかし";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slab"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Revit.Elements.Element ChangeSlabLevelParam_V1(
      Revit.Elements.Element slab,
      double value )
    {
      var e = slab.InternalElement;

      double unit = UnitUtils.Convert(
        value,
        DisplayUnitType.DUT_MILLIMETERS,
        DisplayUnitType.DUT_DECIMAL_FEET );

      Document doc = DocumentManager.Instance.CurrentDBDocument;

      TransactionManager.Instance.EnsureInTransaction( doc );
      e.get_Parameter( BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM )
        .Set( unit );
      TransactionManager.Instance.TransactionTaskDone();

      return slab;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slab"></param>
    /// <returns></returns>
    public static Revit.Elements.Element SetTagLevelFromSlab_V1(
      Revit.Elements.Element slab )
    {
      var e = slab.InternalElement;
      var level_con = e
        .get_Parameter( BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM )
        .AsDouble();

      var unit = UnitUtils.Convert(
        level_con,
        DisplayUnitType.DUT_DECIMAL_FEET,
        DisplayUnitType.DUT_MILLIMETERS );
      var plusValue = String.Format( "{0}{1}", "+", unit );

      Document doc = DocumentManager.Instance.CurrentDBDocument;

      TransactionManager.Instance.EnsureInTransaction( doc );
      if ( unit > 0 )
        e.LookupParameter( _param_name1 ).Set( plusValue );
      else
        e.LookupParameter( _param_name1 ).Set( unit.ToString() );
      TransactionManager.Instance.TransactionTaskDone();

      return slab;
    }


  }
}
