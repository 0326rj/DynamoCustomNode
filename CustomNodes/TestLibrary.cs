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
  public class TestLibrary
  {
    private TestLibrary()
    {
    }

    private Autodesk.Revit.DB.Category _revitCategory;
    private Revit.Elements.Category _dynamoCategory;


    public static bool isSlabCategory(Revit.Elements.Element inRevitSlab )
    {
      bool flag;

      var unWrappedSlab = inRevitSlab.InternalElement;

      var revitCategory = unWrappedSlab.Category.Name;
      var dynaCategory = inRevitSlab.GetCategory.ToString();

      if ( revitCategory.Equals( dynaCategory ) ) 
        flag = true;
      else
        flag = false;

      return flag;
    }


    public static string ViewDyAndRvCategory( Revit.Elements.Element inRevitSlab )
    {
      var unWrappedSlab = inRevitSlab.InternalElement;

      var revitCategory = unWrappedSlab.Category.Name;
      var dynaCategory = inRevitSlab.GetCategory.ToString();

      var bp_level = unWrappedSlab
        .get_Parameter( BuiltInParameter.LEVEL_PARAM )
        .AsValueString();
      var bp_levelOffset = unWrappedSlab
        .get_Parameter( BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM )
        .AsValueString();
      

      string result = String
        .Format( "Revit Category : {0}\nDynamo Category : {1}\nLEVEL_PARAM : {2}\nLEVEL_OFFSET : {3}"
        , revitCategory, dynaCategory, bp_level, bp_levelOffset );

      return result;
    }


    public static Revit.Elements.Element SetSlabTagParameter(
      Revit.Elements.Element inRevitSlab )
    {
      const string param_name1 = "床スラブ_CON天端レベル";
      const string param_name2 = "床スラブ_構造体天端レベル";

      var unwrapped = inRevitSlab.InternalElement;
      var btc_floor = unwrapped.Category;
      var btp_levelOffset = unwrapped
        .get_Parameter( BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM )
        .AsDouble();
      

      // 床カテゴリ判定
      if ( unwrapped.Category.Id.IntegerValue == (-200032) ) 
      {
        Document doc = DocumentManager.Instance.CurrentDBDocument;

        TransactionManager.Instance.EnsureInTransaction( doc );



        TransactionManager.Instance.TransactionTaskDone();
      }

      return inRevitSlab;
    }


    // LookupParameter( string ) method Test 2
    // パラメータ IDを習得する
    public static string GetParameterId(
      Revit.Elements.Element elements)
    {
      const string param_name1 = "床スラブ_CON天端レベル";

      var n = elements.InternalElement;
      var p = n.LookupParameter( param_name1 );
      var id = p.Id.IntegerValue.ToString();

      return id;
    }


    public static Revit.Elements.Element SetParameter(
      Revit.Elements.Element slab,
      double value)
    {
      const string param_name1 = "床スラブ_CON天端レベル";
      
      var e = slab.InternalElement;

      Document doc = DocumentManager.Instance.CurrentDBDocument;

      TransactionManager.Instance.EnsureInTransaction( doc );
      e.LookupParameter( param_name1 ).Set( value.ToString() );
      TransactionManager.Instance.TransactionTaskDone();

      return slab;
    }


   
  }
}
