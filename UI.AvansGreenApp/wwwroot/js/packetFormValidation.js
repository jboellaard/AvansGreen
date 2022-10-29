window.onload = (event) => {
    initMultiselect();
};

function initMultiselect() {
    checkboxStatusChange();

    document.addEventListener("click", function (evt) {
        var flyoutElement = document.getElementById('myMultiselect'),
            targetElement = evt.target; 

        do {
            if (targetElement == flyoutElement) {
                return;
            }
            targetElement = targetElement.parentNode;
        } while (targetElement);

        toggleCheckboxArea(true);
    });
}

function checkboxStatusChange() {
    var multiselect = document.getElementById("mySelectLabel");
    var selectedOptionsAbove = document.getElementById("selectedOptions");
    var multiselectOption = multiselect.getElementsByTagName('option')[0];

    var values = [];
    var labeltexts = [];
    var checkboxes = document.getElementById("mySelectOptions");
    var checkedCheckboxes = checkboxes.querySelectorAll('input[type=checkbox]:checked');

    for (const item of checkedCheckboxes) {
        var label = document.querySelector("label[for='" + item.getAttribute('id') + "']").textContent;
        labeltexts.push(label);
    }

    var dropdownValue = "Nothing is selected";
    var labels = "Selected products: ";
    if (labeltexts.length > 0) {
        if (labeltexts.length > 1) dropdownValue = labeltexts.length + " products";
        else dropdownValue = "1 product";
        labels += labeltexts.join(', ');
    }

    multiselectOption.innerText = dropdownValue;
    selectedOptionsAbove.innerText = labels;
}

function toggleCheckboxArea(onlyHide = false) {
    var checkboxes = document.getElementById("mySelectOptions");
    var displayValue = checkboxes.style.display;

    if (displayValue != "block") {
        if (onlyHide == false) {
            checkboxes.style.display = "block";
        }
    } else {
        checkboxes.style.display = "none";
    }
}

var timeError = document.getElementById("pickupTimeError");

$("input[type=time]").on("change", function () {
    timeError.innerHTML = "";
});

$("#pickupTimeStart").on("change", function () {
    var timeval = new Date(this.valueAsDate);
    var endInput = document.querySelector("#pickupTimeEnd");
    if (endInput.value != "") {
        var end = new Date(endInput.valueAsDate);
        if (timeval >= end) {
            timeError.innerHTML = "The ending time should be after the start.";
        }
    }
});
$("#pickupTimeEnd").on("change", function () {
    var timeval = new Date(this.valueAsDate);
    var startInput = document.querySelector("#pickupTimeStart");
    if (startInput.value != "") {
        var start = new Date(startInput.valueAsDate);
        if (timeval <= start) {
            timeError.innerHTML = "The ending time should be after the start.";
        }
    }
});

$("input[type=time]").on("change", function () {
    var timeval = new Date(this.valueAsDate);
    console.log(timeval.getUTCHours());
    if (!((timeval.getUTCHours() < 20 && timeval.getUTCHours() >= 8) || (timeval.getUTCHours() == 20 && timeval.getUTCMinutes() == 0))) {
        console.log("hello");
        timeError.innerHTML = "<p>Please enter a time after 08:00 and before 20:00.</p>";
    }
});

$("button[type=submit]").on("click", function (e) {
    $('.checkmodal').modal("hide");
});

$("#modalid").on("click", function (e) {
    var modalELement = $(".checkmodal");
    if (!timeError.innerHTML.length == 0) {
        e.preventDefault();
    } else {
        modalELement.modal("toggle");
    }
});