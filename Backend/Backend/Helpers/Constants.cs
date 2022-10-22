using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class Constants
    {
        public static string DirectoryPayments = "\\uploadpayments\\";
        public static string DirectoryPaymentsBill = "\\uploadpaymentBils\\";
        public static string DirectoryRooms = "\\uploadrooms\\";
        public static string RoomsImage = "room" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
        public static string PaymentsImage = "payment" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
        public static string PaymentsBillImage = "paymentBill" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
    }
}
