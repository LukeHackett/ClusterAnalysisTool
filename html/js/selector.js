jQuery(document).ready(function($){

  var listings = [];


  $.getJSON("data/listings.json", function(data) {
    // Add each file name to the list of files
    $.each(data, function(j, entry){
      // Make a copy of the returned data
      listings[entry.name] = entry;
      $("#files ul").append('<li><a href="#">' + entry.name + '</a></li>');
    });
  });

  /**
   * 
   */
  $("#files").on("click", "a", function(){
    var self = $(this);
    var mode = self.text();

    // Update the File Section
    $("#files button:first").text(mode);

    // Remove any current graphs
    $("#weekly > #drop_rat").children().not("ul").remove();
    $("#weekly > #drop_mix_band").children().not("ul").remove();
    $("#weekly > #fail_rat").children().not("ul").remove();
    $("#weekly > #fail_mix_band").children().not("ul").remove();

    $("#product > #drop_rat").children().not("ul").remove();
    $("#product > #drop_mix_band").children().not("ul").remove();
    $("#product > #fail_rat").children().not("ul").remove();
    $("#product > #fail_mix_band").children().not("ul").remove();

    $("#weekly ul").children().remove();
    $("#product ul").children().remove();

    // Create some new graphs
    var entry = listings[mode];

    $.each(entry.week, function(i, week){
      // obtain the values
      var file = week.file;
      var container = week.container;
      // Create the bar chart
      CreateChart(file, container);
    });
    
    // Create all graphs for the Product based data
    $.each(entry.product, function(i, product){
      // obtain the values
      var file = product.file;
      var container = product.container;
      // Create the bar chart
      CreateChart(file, container);
    });

  });


  /**
   * Handles the Mode Switcher drop down found upon the top right-hand side of 
   * page.
   */
  $("#analysis").on("click", "a", function(){
    var self = $(this);
    var mode = self.text();

    // Swap the mode
    if(mode == "Product"){
      // Fade out the Weekly view
      $("#weekly").fadeOut();
      // Fade in the Product view
      $("#product").fadeIn();
      // Set the mode within the button
      $("#analysis button:first").text(mode);

    } else if(mode == "Weekly"){
      // Fade out the product view
      $("#product").fadeOut();
      // Fade in the Weekly view
      $("#weekly").fadeIn();
      // Set the mode within the button
      $("#analysis button:first").text(mode);
    }

  });

});