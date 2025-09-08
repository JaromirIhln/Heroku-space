window.initBootstrapDropdowns = () => {
    // Najde všechny dropdowny a inicializuje je
    var dropdownElementList = [].slice.call(document.querySelectorAll('.dropdown-toggle'));
    dropdownElementList.forEach(function (dropdownToggleEl) {
        if (window.bootstrap && window.bootstrap.Dropdown) {
            new window.bootstrap.Dropdown(dropdownToggleEl);
        }
    });
};
