
var bebop = require('node-bebop');
var ffmpeg = require('fluent-ffmpeg');
var drone = bebop.createClient();
var fs = require("fs");
var output = fs.createWriteStream("drone.mov");
var tmp=0;
var picturecount=0;
var jpegoutput = fs.createWriteStream("drone.jpeg");

//const readline = require('readline');

//readline.emitKeypressEvents(process.stdin);
//process.stdin.setRawMode(true);

drone.connect(function() {
  //console.log("1 : takeoff     2 : land      3 : video on     4 : video off     5 : picture\nw : forward     s : backward     a : left     d : right\mq : left turn     e : right turn");
    str = process.argv[2];
    //console.log(str)
    //console.log(key)
    if(str=='w'){
      //console.log(str);
      drone.forward(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.forward(0);
      //}
    }
    else if(str=='s'){
      //console.log(str);
      drone.back(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.back(0);
      //}
    }
    else if(str=='a'){
      //console.log(str);
      drone.left(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.left(0);
      //  drone.stop();
      //}
    }
    else if(str=='d'){
      //console.log(str);
      drone.right(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.right(0);
      //  drone.stop();
      //}
    }
    else if(str=='q'){
      //console.log(str);
      drone.counterClockwise(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.counterClockwise(0);
      //}
    }
    else if(str=='e'){
      //console.log(str);
      drone.clockwise(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.Clockwise(0);
      //}
    }
    else if(str=='f'){
      //console.log(str);
      drone.up(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.up(0);
      //}
    }
    else if(str=='g'){
      //console.log(str);
      drone.down(1);
      //for(tmp=0;tmp<1000000;tmp++){
      //  drone.down(0);
      //}
    }
    else if(str=='1'){
      //console.log(str);
      drone.takeOff();
    }
    else if(str=='2'){
      //console.log(str);
      drone.land();
    }
    else if(str=='3'){
      video = drone.getVideoStream();
      drone.MediaStreaming.videoEnable(1);
      video.pipe(output);
      //console.log(str);
    }
    else if(str=='4'){
      drone.MediaStreaming.videoEnable(0);
      //console.log(str);
    }
    else if(str=='5'){
      //picture
      drone.MediaStreaming.videoEnable(1);
      drone.takePicture();
      jpg = drone.getMjpegStream();
      jpg.pipe(jpegoutput);
      drone.MediaStreaming.videoEnable(0);

      //console.log(str);
    }
    else if(str=='6'){
      //picture
      //console.log(str);
      drone.MediaStreaming.videoEnable(0);
    }
    else if(str=='o'){
      drone.stop();
    }
    
    setTimeout(function() {
      process.exit();
    }, 800);
});

