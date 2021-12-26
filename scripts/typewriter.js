var i = 0;
var txt = 'Lorem ipsum dummy text blabla.';
var speed = 50;

function typeWriter() {
  if (i < txt.length) {
    document.getElementById("demo").innerHTML += txt.charAt(i);
    i++;
    setTimeout(typeWriter, speed);
  }
}
/*
<script>
    var i = 0;
    var txt = '@Model.Content.Title';
    var speed = 100;
    
    function typeWriter() {
        if (i < txt.length) {
            document.getElementById("header-title").innerHTML += txt.charAt(i);
            i++;
            setTimeout(typeWriter, speed);
        }
    }
    //window.onload = typeWriter; // deactivated
</script>
*/