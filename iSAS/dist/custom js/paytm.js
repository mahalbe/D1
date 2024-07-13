
<script type="application/javascript" crossorigin="anonymous" src="https://securegw-stage.paytm.in/merchantpgpui/checkoutjs/merchants/PyVhRr64273223302829.js"
    onload="MakeOnlinePayment1()"></script>



function MakeOnlinePaymentByPaytmStandard() {
    $.ajax({
        url: '../StudentFeeDetails/MakeOnlinepayment',
        data: { paymentMode: "Online", erpno: '@Model.ERP', amount: '@totalPayableAmount' },
        type: 'get',
        success: (function (response) {
            if (response.status == 'success') {
                window.location.href = "/StudentFeeDetails/MakePayment?tnxId=" + response.transactionNo + "&amount=" + '@totalPayableAmount';
            }
            else {
                OpenMessegeAutoHideDiv(response.Msg, response.Color);
            }
        })
    });
};
function MakeOnlinePayment() {
    $.ajax({
        url: '../StudentFeeDetails/MakeOnlinepayment',
        data: { paymentMode: "Online", erpno: '@Model.ERP', amount: '@totalPayableAmount' },
        type: 'get',
        success: (function (response) {


            if (response.status == 'success') {
                //DoAjaxPostAndMore(response.transactionNo, response.erpNumber);
                //window.location.href = "/StudentFeeDetails/MakePayment1?tnxId=" + response.transactionNo + "&amount=" + '@totalPayableAmount';

                onScriptLoad(response.tokenNo, '@totalPayableAmount', response.orderId);
                //onScriptLoad('16633b187b3a43d3a7037fc5108b54da1608733278322','@totalPayableAmount','35694f82-3c9a-436e-b73e-88943a11796520205123195117');
            }
            else {
                OpenMessegeAutoHideDiv(response.Msg, response.Color);
            }
        })
    });
};
function MakeOnlinePayment1() {
    onScriptLoad('', '8100', '');
};
function Response(responsedata) {
    $.ajax({
        url: '../StudentFeeDetails/Response_',
        data: { data: responsedata },
        type: 'post',
        success: (function (response) {
            if (response.status == 'success') {

            }
            else {
                OpenMessegeAutoHideDiv(responsedata.RESPMSG, "warning");
            }
        })
    });
};

function onScriptLoad(token, amt, order) {
    alert(token);
    alert(amt);
    alert(order);
    //spinnerShow();
    var config = {
        "root": "",
        "flow": "DEFAULT",
        "data": {
            "orderId": order,
            "token": token,
            "tokenType": "TXN_TOKEN",
            "amount": amt
        },
        merchant: {
            redirect: false
        },
        "handler": {
            notifyMerchant: function (eventName, data) {
                console.log("notifyMerchant handler function called");
                console.log("eventName => ", eventName);
                console.log("data => ", data);
                if (eventName == 'SESSION_EXPIRED') {
                    location.reload();
                }
            },
            transactionStatus: function (data) {

                console.log("payment status ", data);
                window.Paytm.CheckoutJS.close();
                Response(data);


                //var result = "<h2>Response: </h2><table>";
                //for (var key in data) {
                //    if (data.hasOwnProperty(key)) {
                //        result += "<tr><td>" + key + "</td><td>" + data[key] + "</td></tr>";
                //    }
                //}
                //result += "</table>";
                //document.getElementById("blink-response").innerHTML = result;



            }
        }
    };

    if (window.Paytm && window.Paytm.CheckoutJS) {

        window.Paytm.CheckoutJS.onLoad(function excecuteAfterCompleteLoad() {
            // initialze configuration using init method

            window.Paytm.CheckoutJS.init(config).then(function onSuccess() {
                //spinnerHide();
                // after successfully updating configuration, invoke Blink Checkout
                console.log("I am in js");
                window.Paytm.CheckoutJS.invoke();


            }).catch(function onError(error) {
                //console.log("I am in error");
                console.log("error => ", error);
            });
        });
    }
}