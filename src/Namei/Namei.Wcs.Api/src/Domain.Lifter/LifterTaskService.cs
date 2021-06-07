// using System.IO;
// using System.Linq;
// using Namei.Wcs.Api;

// namespace Namei.Wcs.Aggregates
// {
//   public class LifterDomainService: ILifterDomainService
//   {
//     private readonly ILifterServiceManager _lifterManager;

//     private readonly ILifterTaskRepository _lifterRepository;
    
//     public LifterDomainService(
//       ILifterServiceManager lifterManager,
//       ILifterTaskRepository lifterTaskRepository
//     ) {
//       _lifterManager = lifterManager;
//       _lifterRepository = lifterTaskRepository;
//     }

//     public void Imported(LifterTaskImported param)
//     {
//       var lifterFloorService = _lifterManager.GetLifterFloor(param.LifterId, param.Floor);

//       if (param.Barcode != null) {
//         var task = LifterTask.FromImportedEvent(param);

//         _lifterRepository.Add(task);
//       }

//       lifterFloorService.SetImported(param.LifterId, param.Floor);
//     }

//     public void Scanned(string lifterId, string floor)
//     {
//       var lifterFloorService = _lifterManager.GetLifterFloor(lifterId, floor);
//       var barcode = lifterFloorService.GetBarcode();
//       var destination = lifterFloorService.GetExecutingDestination();
//       var task = _lifterRepository.FindFromBarcode(barcode);

//       if (task == null) {

//       }
//     }

//     public void Exported(string lifterId, string floor)
//     {

//     }

//     public void Finished(string lifterId, string floor)
//     {

//     }
//   }
// }
