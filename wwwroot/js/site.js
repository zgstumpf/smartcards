
// Search and Filter Functionality for the Sets Page
$(function() {
    $('#setSearch').on('input', function() {
        var value = $(this).val().toLowerCase();
        $('.col').filter(function() {
            var setName = $(this).find("h2").text().toLowerCase();
            $(this).toggle(setName.indexOf(value) > -1);
        });
    });
    $('#setFilter').on('change', function() {
        var value = $(this).val();
        $('.card').filter(function() {
            $(this).toggle(value === '' || $(this).data('category') === value);
        });
    });
});

$(document).ready(function() {
    // when a checkbox is clicked
    $('input[type=checkbox]').click(function() {
      // create an array of checked category values
      var checked = $('input[type=checkbox]:checked').map(function() {
        return this.value;
      }).get();
      
      // show all cards if no categories are checked
      if (checked.length === 0) {
        $('.col').show();
      } else {
        // hide cards that don't have a checked category
        $('.col').each(function() {
          if (checked.indexOf($(this).find('.card-body h3').text()) >= 0) {
            $(this).show();
          } else {
            $(this).hide();
          }
        });
      }
    });
  });  

$(document).ready( function () {
    $('#cardTable').DataTable();

    $('#undecided-and-granted-promotion-requests').DataTable({
        order: [[1, 'desc']], // When table is loaded, show most recent promotion requests first
    });
    $('#undecided-requests').DataTable({
        order: [[1, 'desc']], 
    });

    $('#report-index-table').DataTable(
        {
            "columnDefs": [
                { "type": "num", "targets": [1, 2] }
            ],
            "columns": [
                null,  // assume first column is not numeric
                { "data": 1, "type": "num" },  // specify that second column is numeric
                { "data": 2, "type": "num" },  // specify that third column is numeric
                null   // assume remaining columns are not numeric
            ]
        }
    );

});

function GetAverageRating(SetId) {
    $.ajax({
        url: "/SetRatingv3/AverageRating?SetId=" + SetId,
        type: "POST",
        dataType: "text",
        success: function (result) {
            result = parseFloat(result).toFixed(2);
            html = `
            <label for="rating-bar">${result}</label>
            <meter id="rating-bar" value="${result}" min="0" max="5"></meter><br>
            `
            $('#average-rating-in-report-details').html(html);
        },
        error: function (xhr, status, error) {
            console.log("An error occurred while getting the average rating: " + error);
        }
    });
}