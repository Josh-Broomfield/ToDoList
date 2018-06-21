function addNoteClick() {
    var $button = $(this);
    var $parent = $button.parent().parent();
    var $toggle = $parent.children(".toggleMe");
    var $deleteField = $toggle.children().eq(2);

    //toggle hidden Delete option value
    if ($deleteField.val() === 'True') {
        $deleteField.val('False');
    } else {
        $deleteField.val('True');
    }

    //toggle text
    if ($button.text() === 'Delete') {
        $button.text('Undo');
    } else {
        $button.text('Delete');
    }

    $toggle.toggle();
}

$(document).ready(function () {
    $(".datefield").datepicker({ dateFormat: 'M dd, yy' });
    //$(".datefield").datepicker("option", "dateFormat", "M dd, yy");//http://api.jqueryui.com/datepicker/#option-dateFormat http://api.jqueryui.com/datepicker/#utility-formatDate

    $(".deleteNote").on("click", addNoteClick);

    $(".addNote").on("click", function () {
        $.ajax({
            type: "GET",
            async: true,
            url: '/Note/PartialEdit',
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (partialView) {
                $('.notesContainer').append(partialView);

                $(".deleteNote").off("click");//just in case there are any complications with adding this more than once
                $(".deleteNote").on("click", addNoteClick);
            },
            fail: function (asd) {
                alert('Failed to add a new note');
            }
        });
    });

    $(".notesContainer.deleteNote").on("click", function () {
        console.log('success!');
    });
});