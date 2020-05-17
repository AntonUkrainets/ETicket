using System;
using System.Reflection;
using ETicket.Admin.Models.DataTables;
using ETicket.ApplicationServices.Services.DataTable.Interfaces;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class TransactionHistoryController : Controller
    {
        #region Private Members

        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDataTableService<TransactionHistory> dataTableService;
        private readonly ITransactionService transactionAppService;

        #endregion

        public TransactionHistoryController(ITransactionService transactionAppService, IDataTableService<TransactionHistory> dataTableService)
        {
            this.transactionAppService = transactionAppService;
            this.dataTableService = dataTableService;
        }

        public IActionResult Index()
        {
            logger.Info(nameof(TransactionHistoryController.Index));

            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetCurrentPage([FromQuery]DataTablePagingInfo pagingInfo)
        {
            return Json(dataTableService.GetDataTablePage(pagingInfo));
        }

        public IActionResult Details(Guid? id)
        {
            logger.Info(nameof(TransactionHistoryController.Details));

            if (id == null)
            {
                logger.Warn(nameof(TransactionHistoryController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var transaction = transactionAppService.GetTransactionById(id.Value);

                if (transaction == null)
                {
                    logger.Warn(nameof(TransactionHistoryController.Details) + " transaction is null");

                    return NotFound();
                }

                return View(transaction);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return BadRequest();
            }
        }
    }
}