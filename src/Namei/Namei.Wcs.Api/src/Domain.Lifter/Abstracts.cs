// using Namei.Wcs.Api;

// namespace Namei.Wcs.Aggregates
// {
//   public interface ILifterDomainService
//   {
//     void Imported(LifterTaskImported param);

//     void Scanned(string lifterId, string floor);

//     void Exported(string lifterId, string floor);

//     void Finished(string lifterId, string floor);
//   }

//   public interface ILifterTaskRepository
//   {
//     void Add(LifterTask task);

//     void Update(LifterTask task);

//     LifterTask FindFromBarcode(string barcode);
//   }

//   public interface ILifterServiceManager
//   {
//     ILifterService GetLifter(string lifterId);

//     ILifterFloorService GetLifterFloor(string lifterId, string floor);
//   }

//   public interface ILifterService
//   {
//     ILifterFloorService GetFloor(string floor);
//   }

//   public interface ILifterFloorService
//   {
//     void SetImported(string barcode = null, string destination = null);

//     void SetDestination(string destination);

//     string GetExecutingDestination();

//     string GetBarcode();
//   }
// }
