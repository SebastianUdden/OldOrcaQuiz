(function (ng) {
    var treeServiceApp = ng.module('tree.factory', []);
    treeServiceApp.factory("URLConfig", [function () {
        return {
            tree: "api/tree.json"
        }
    }]);
})(angular);
