using Content.Shared.Eye.Blinding;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;


namespace Content.Client.Eye.DarkVision;
public sealed class DarkVisionOverlay : Overlay
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] IEntityManager _entityManager = default!;
    [Dependency] ILightManager _lightManager = default!;


    public override bool RequestScreenTexture => true;
    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly ShaderInstance _greyscaleShader;
    //private readonly ShaderInstance _circleMaskShader;

    private DarkVisionComponent _darkVisionComponent = default!;

    public DarkVisionOverlay()
    {
        IoCManager.InjectDependencies(this);
        _greyscaleShader = _prototypeManager.Index<ShaderPrototype>("GreyscaleFullscreen").InstanceUnique();
        //_circleMaskShader = _prototypeManager.Index<ShaderPrototype>("CircleMask").InstanceUnique();
    }
    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        //_lightManager.Enabled = true;
        return true;

        /*
        if (!_entityManager.TryGetComponent(_playerManager.LocalPlayer?.ControlledEntity, out EyeComponent? eyeComp))
            return false;

        if (args.Viewport.Eye != eyeComp.Eye)
            return false;

        var playerEntity = _playerManager.LocalPlayer?.ControlledEntity;

        if (playerEntity == null)
            return false;

        //if (!_entityManager.TryGetComponent<BlindableComponent>(playerEntity, out var blindComp))
        //    return false;

        //blindableComponent = blindComp;

        var blind = _blindableComponent.Sources > 0;

        if (!blind && _blindableComponent.LightSetup) // Turn FOV back on if we can see again
        {
            _lightManager.Enabled = true;
            _blindableComponent.LightSetup = false;
            _blindableComponent.GraceFrame = true;
            return true;
        }

        return blind;
        */
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture == null)
            return;
        //_lightManager.DrawHardFov = true;
        _lightManager.DrawLighting = false;
        //_lightManager.DrawShadows = false;


        _greyscaleShader?.SetParameter("SCREEN_TEXTURE", ScreenTexture);

        var worldHandle = args.WorldHandle;
        var viewport = args.WorldBounds;
        worldHandle.UseShader(_greyscaleShader);
        worldHandle.DrawRect(viewport, Color.DarkKhaki);
        worldHandle.UseShader(null);
    }
}
