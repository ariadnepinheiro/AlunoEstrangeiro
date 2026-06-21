$(document).ready(function () {
    $('.dropdown-submenu a.test').on("click", function (e) {
        $(this).next('ul').toggle();
        e.stopPropagation();
        e.preventDefault();
    });

    $('.top_inf').hover(
        () => { $("#top_menu").css("display", "block"); },
        () => { $("#top_menu").css("display", "none"); }
    );
});



  habilitarDragRybena();   
  
function habilitarDragRybena() {

  function inicializarArrastorybena($sidebar) {
    // Criei uma div externba do componente pra nao interferir nos eventos do componente e inserindo css pra ficar fixed na direita.
    // no caso deixando arrastar apenas pra cima e baixo. Mudando apenas o eixo X 
    const $container = $('<div id="rybena-draggable-container"></div>').css({
      position: 'fixed',
      top: $sidebar.offset().top + 'px',
      right: '0px', 
      left: 'auto',  
      zIndex: 99999,
      display: 'block',
      margin: '0',
      padding: '0'
    });

    // Envolve o componente do Rybena
    $sidebar.wrap($container);
    const $dynContainer = $('#rybena-draggable-container');

    let isDragging = false;
    let startY = 0;
    let currentY = 0; 
    let accumulatedY = 0; 
    let hasMoved = false;
    const dragThreshold = 5;
    
    // preventdefault impede que seja feito um postback 
    // Remove impedimentos de arrasto nativo do navegador
    $dynContainer.on('dragstart selectstart', function(e) { e.preventDefault(); });
    
    //ao pressionar cria o evento rxDrag
    $dynContainer.on('mousedown', function(e) {
      if (e.button !== 0) return; // Apenas botão esquerdo

      isDragging = true;
      hasMoved = false;
      startY = e.clientY;

      $(document).on('mousemove.rxDrag', function(me) {
        if (!isDragging) return;

        const deltaY = me.clientY - startY;

        if (Math.abs(deltaY) > dragThreshold) {
          hasMoved = true;
        }

        if (hasMoved) {
          currentY = accumulatedY + deltaY;
          // Força o movimento estritamente vertical via Transform
          $dynContainer[0].style.setProperty('transform', `translateY(${currentY}px)`, 'important');
        }
      });
            
      //ao soltar o botao, destroi os eventos criados
      $(document).on('mouseup.rxDrag', function() {
        isDragging = false;
        $(document).off('mousemove.rxDrag mouseup.rxDrag');

        if (hasMoved) {
          accumulatedY = currentY; 

          // Bloqueia o clique fantasma nos botões internos
          window.addEventListener('click', function block(e) {
            e.preventDefault();
            e.stopPropagation();
            window.removeEventListener('click', block, true);
          }, true);
        }
      });
    });
  }

  // --- OBSERVER: Aguarda o Rybena criar a div no DOM ---
  const alvo = document.body;
  const observador = new MutationObserver(function(mutations) {
  const $sidebar = $('#rybena-sidebar');
    
    if ($sidebar.length && !$('#rybena-draggable-container').length) {
      inicializarArrastorybena($sidebar);
      observador.disconnect(); // Finaliza o observer após cumprir a missão
    }
  });

  observador.observe(alvo, { childList: true, subtree: true });
}