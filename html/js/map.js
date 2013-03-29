jQuery(document).ready(function($){

  /**
   * This function will update the navigation bar with all the available KMZ 
   * listings.
   */
  $.getJSON("data/listings.json", function(data) {
    // Loop over all KMZ files
    $.each(data, function(i, entry){
      // Add an KMZ entry to the Data Files
      var file = entry.kmz;
      var id = "data" + i;
      // Append
      $("#options-nav").append('<li> \
                                  <label class="checkbox"> \
                                    <input type="checkbox" id="' + id + '" onclick="toggleKml(this, \'' + file + '\')" /> \
                                    ' + entry.name + ' \
                                  </label> \
                                </li>');
    });
  });
});

  // Base Server URL
  var SERVER_URL = "http://jarvis/d3/";

  // Store the object loaded for the given file.
  var currentKmlObjects = { };

  // Google Earth Instance
  var ge;
  google.load("earth", "1");


  /**
   * This function will return the BaseURL of the web application.
   * NOTE: This function is a **little hacky**. It will only work on the 
   * map.html file.
   */
  function getBaseURL() {
    return window.location.href.replace(/map.html/gi, '');
  }


  /**
   * Initialisation
   */
  function init() {
    google.earth.createInstance('map', initCB, failureCB);
  }


  /**
   * Callback Instance (success)
   */
  function initCB(instance) {
    ge = instance;
    ge.getWindow().setVisibility(true);

    // Enable Google Earth Options
    toggleCompass();
    toggleRoads();
    toggleBoarders();
  }


  /**
   * Callback Instance (failure)
   */
  function failureCB(errorCode) {
    throw new Error(errorCode);
  }


  /**
   * Toggles the visibility of the compass controls
   */
  function toggleCompass() {
    if (compass.checked) {
      ge.getNavigationControl().setVisibility(ge.VISIBILITY_AUTO);
    } else {
      ge.getNavigationControl().setVisibility(ge.VISIBILITY_HIDE);
    }
  }


  /**
   * Toggles the visibility of the road names
   */
  function toggleRoads() {
    ge.getLayerRoot().enableLayerById(ge.LAYER_ROADS, roads.checked);
  }


  /**
   * Toggles the visibility of the country borders
   */
  function toggleBoarders() {
     ge.getLayerRoot().enableLayerById(ge.LAYER_BORDERS, borders.checked);
  }


  /**
   * Toggle the visibility of a given KML file
   */
  function toggleKml(checkbox, file) {
    // remove the old KML object if it exists
    if (currentKmlObjects[file]) {
      ge.getFeatures().removeChild(currentKmlObjects[file]);
      currentKmlObject = null;
    }

    // if the checkbox is checked, fetch the KML and show it on Earth
    if (checkbox.checked) {
      loadKml(file);
    }
  }

  /**
   * Load a given KML file
   */
  function loadKml(file) {
    // Create the FULL URL
    var kmlUrl = getBaseURL() + file;
    // fetch the KML
    google.earth.fetchKml(ge, kmlUrl, function(kmlObject) {
      // NOTE: we still have access to the 'file' variable (via JS closures)
      if (kmlObject) {
        // show it on Earth
        currentKmlObjects[file] = kmlObject;
        ge.getFeatures().appendChild(kmlObject);
      } else {
        // bad KML
        currentKmlObjects[file] = null;
        // wrap alerts in API callbacks and event handlers
        // in a setTimeout to prevent deadlock in some browsers
        setTimeout(function() {
          alert('Bad or null KML.');
        }, 0);
      }
    });
  }

  // Run
  google.setOnLoadCallback(init);

