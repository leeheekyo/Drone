//console.log(process.argv);
//console.log(process.argv[2]);

var bebop = require("node-bebop"),
      fs = require("fs");

var output = fs.createWriteStream(process.argv[2]+".jpeg"),
    drone = bebop.createClient();

drone.connect(function() {
  drone.MediaStreaming.videoEnable(1);
  var video = drone.getMjpegStream();
  video.pipe(output);
  
  //var i;
  //for(i=0; i<10000; i++) ;
  drone.MediaStreaming.videoEnable(0);
  setTimeout(function() {
    process.exit();
  //}, 800);
  }, 10000);

});

