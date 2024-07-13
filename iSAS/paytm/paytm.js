
    function onScriptLoad(orderid, token, amount) {
        var config = {
        "root": "",
    "flow": "DEFAULT",
            "data": {
        "orderId": orderid, /* update order id */
    "token": token, /* update token value */
    "tokenType": "TXN_TOKEN",
    "amount": amount /* update amount */
},
            "handler": {
        "notifyMerchant": function (eventName, data) {
        console.log("notifyMerchant handler function called");
    console.log("eventName => ", eventName);
    console.log("data => ", data);
}
}
};

        if (window.Paytm && window.Paytm.CheckoutJS) {
        window.Paytm.CheckoutJS.onLoad(function excecuteAfterCompleteLoad() {
            // initialze configuration using init method
            window.Paytm.CheckoutJS.init(config).then(function onSuccess() {
                // after successfully updating configuration, invoke Blink Checkout
                window.Paytm.CheckoutJS.invoke();
            }).catch(function onError(error) {
                console.log("error => ", error);
            });
        });
    }
}

