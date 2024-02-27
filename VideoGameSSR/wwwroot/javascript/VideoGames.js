function RefreshPage() {
    location.reload();
}

function SortVideoGamesBy(sortBy) {
    $("#sort-by-value-holder").val(sortBy);
    $("#video-games-orig-body").addClass("d-none");
    $("#video-games-loading-body").removeClass("d-none");
    $("#video-games-loading-body").addClass("d-block");
    $("#SortVideoGamesForm").submit();
}

function ShowButtonSpinner(mode) {
    if (mode === "Add") {
        $("#add-game-button-container").removeClass("d-block");
        $("#add-game-button-container").addClass("d-none");
        $("#add-game-loading").removeClass("d-none");
        $("#add-game-loading").addClass("d-block");
    } else if (mode === "Edit") {
        $("#edit-game-button-container").removeClass("d-block");
        $("#edit-game-button-container").addClass("d-none");
        $("#edit-game-loading").removeClass("d-none");
        $("#edit-game-loading").addClass("d-block");
    } else {
        $("#delete-game-button-container").removeClass("d-block");
        $("#delete-game-button-container").addClass("d-none");
        $("#delete-game-loading").removeClass("d-none");
        $("#delete-game-loading").addClass("d-block");
    }
}

// Open Modal Region
// #region 
function OpenEditModal(id, title, publisher, release_year) {
    var formattedId = parseInt(id);
    var formattedYear = parseInt(release_year);

    $('#edit-game-id').val(formattedId);
    $('#edit-title').val(title);
    $('#edit-publisher').val(publisher);
    $('#edit-release-year').val(formattedYear);
}

function OpenDeleteModal(id) {
    var formattedId = parseInt(id);
    $('#delete-game-id').val(formattedId);
}
// #endregion

// Close Modal Region
// #region 
function CloseAddModal() {
    setTimeout(function () {
        $('#add-title').val("");
        $('#add-publisher').val("");
        $('#add-release-year').val("");
    }, 100);
}

function CloseEditModal() {
    setTimeout(function () {
        $('#edit-modal-title').empty();
        $('#title').empty();
        $('#publisher').empty();
        $('#release_year').empty();
    }, 100);
}

function CloseDeleteModal() {
    setTimeout(function () {
        $('#delete-modal-title').empty();
    }, 100);
}
// #endregion