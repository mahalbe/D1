function MakeOnlinePayment(erpno, amount,duedate) {
    $.ajax({
        url: '../Fee_OnlinePayment/MakeOnlinepayment',
        data: { paymentMode: "Online", erpno: erpno, amount: amount, duedate: duedate},
        type: 'get',
        beforeSend: function () {
            spinnerShow();
        },
        success: (function (response) {
            if (response.status == 'success') {
                spinnerHide();
              window.open(response.url, "_self");
            }
            else {
                spinnerHide();
                OpenMessegeAutoHideDiv(response.Msg, response.Color);
            }
        })
    });
};

//function redirecToAtom(url) {
//    console.log(url);
//    //var settings = {
//    //    'cache': false,
//    //    'dataType': "jsonp",
//    //    "async": true,
//    //    "crossDomain": true,
//    //    "url": url,
//    //    "method": "GET",
//    //    "headers": {
//    //        "accept": "application/json",
//    //        "Access-Control-Allow-Origin": "*"
//    //    }
//    //}

//    //$.ajax(settings).done(function (response) {
//    //    console.log(response);

//    //});


    
//    $.ajax({
//        url: url,
//        data: {},
//        headers: {
//            'Access-Control-Allow-Origin': 'http://localhost:49932/',
//            'Access-Control-Allow-Methods': 'GET, POST',
//            'Access-Control-Allow-Headers': 'Accepts, Content-Type, Origin, X-My-Header'
//        },
//        //crossDomain: true,
//        //dataType: 'jsonp',
//        type: 'get',
//        success: (function (response) {
//            if (response.status == 'success') {

//                //onScriptLoad(response.tokenNo, '@totalPayableAmount', response.orderId);
//            }
//            else {
//                OpenMessegeAutoHideDiv(response.Msg, response.Color);
//            }
//        })
//    });
//};