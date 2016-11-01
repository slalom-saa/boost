(function (angular) {

	var element;
	var key;

	function block() {
		clearTimeout(key);
		element.className = 'block-ui block-ui-active';
		key = setTimeout(function () {
			element.className = 'block-ui block-ui-active block-ui-wait';
		}, 750);
	}

	function unblock() {
		clearTimeout(key);
		element.className = 'block-ui';
	}

	function init(ui, toastr) {
        // init block-ui
		element = document.createElement('div');
		element.className = 'block-ui';
		document.body.appendChild(element);
		var inner = element.appendChild(document.createElement('div'));
		inner.appendChild(document.createElement('div'));
		inner.appendChild(document.createElement('div'));

	    // init-toastr
		ui.info = toastr.info;
		ui.success = toastr.success;
		ui.info = toastr.info;
		ui.error = toastr.error;
	    ui.warning = toastr.warning;
	}

	angular.module('app').constant('ui', {
		block: block,
		unblock: unblock
	}).run(['ui', 'toastr', function (ui, toastr) {
	    init(ui, toastr);
	}]);

}(angular));