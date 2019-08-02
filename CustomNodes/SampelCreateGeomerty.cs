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
  public class SampelCreateGeomerty
  {
    private SampelCreateGeomerty() { }


    /// <summary>
    /// Curve Geometryからモデル線分を作成します
    /// </summary>
    /// <param name="curves">Input the curves</param>
    /// <returns name="ModelCurves">Curves[][]</returns>
    /// <search> model, curves </search>
    public static List<Revit.Elements.ModelCurve> CreateModelCurves(
      List<Autodesk.DesignScript.Geometry.Curve> curves )
    {
      Document doc = DocumentManager.Instance.CurrentDBDocument;
      TransactionManager.Instance.EnsureInTransaction( doc );

      List<Revit.Elements.ModelCurve> crvOUT = new List<Revit.Elements.ModelCurve>();
      foreach ( var crv in curves )
      {
        var mCrv = Revit.Elements.ModelCurve.ByCurve( crv );
        crvOUT.Add( mCrv );
      }
      TransactionManager.Instance.TransactionTaskDone();

      return crvOUT;
    }
  }
}
