﻿var AnalysisMain = (function() {
    var classAnalysisMain = {};

    classAnalysisMain.main = function () {
        var stockSearchPresenter = new StockSearchPresenter();
        stockSearchPresenter.initializeView();
    };

    return classAnalysisMain;
})();