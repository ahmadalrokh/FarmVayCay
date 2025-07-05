// for search
document.getElementById("searchInput").addEventListener("input", function () {
    var searchString = this.value.toLowerCase();
    var farmsGrid = document.getElementById("farmsGrid");
    var farmCards = farmsGrid.getElementsByClassName("farm-card");
    var noResultsMessage = document.getElementById("noResultsMessage");

    var resultsFound = false;


    for (var i = 0; i < farmCards.length; i++) {
        var farmCard = farmCards[i];
        var farmName = farmCard.getAttribute("data-farm-name").toLowerCase();
        var farmLocation = farmCard.getAttribute("data-farm-location").toLowerCase();

        if (farmName.includes(searchString) || farmLocation.includes(searchString)) {
            farmCard.style.display = "block";
            resultsFound = true;
        } else {
            farmCard.style.display = "none";
        }
    }


    if (!resultsFound) {
        noResultsMessage.style.display = "block";
    } else {
        noResultsMessage.style.display = "none";
    }
});
