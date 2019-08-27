using UnityEditor.VersionControl;

public class TimeOrFrameLimit
{
    public enum Phases
    {
        Render,
        RenderEnd,
        Cooldown,
        CooldownEnd
    }

    public int MinFrame;
    public int MaxFrame;
    public float MinTime;
    public float MaxTime;

    private int _CurrentFrame;
    private float _CurrentTime;

    public Phases CurrentPhase { get; private set; }
    public bool Changed { get; private set; }

    public void Beat(float deltaTime) {
        _CurrentFrame++;
        _CurrentTime += deltaTime;
        Changed = false;

        switch (CurrentPhase) {
            case Phases.Render:
                if (CheckTimeOut()) {
                    SetPhase(Phases.RenderEnd);
                }

                break;
            case Phases.RenderEnd:
                if (_CurrentFrame == 2) {
                    SetPhase(Phases.Cooldown);
                }

                break;
            case Phases.Cooldown:
                if (CheckTimeOut()) {
                    SetPhase(Phases.CooldownEnd);
                }

                break;
            case Phases.CooldownEnd:
                if (_CurrentFrame == 2) {
                    SetPhase(Phases.Render);
                }

                break;
        }
    }

    private void SetPhase(Phases phase) {
        ResetTimers();
        CurrentPhase = phase;
        Changed = true;
    }

    private void ResetTimers() {
        _CurrentFrame = 0;
        _CurrentTime = 0;
    }

    private bool CheckTimeOut() {
        return (_CurrentFrame > MinFrame && _CurrentFrame > MinTime) || _CurrentFrame > MaxFrame || _CurrentTime > MaxTime;
    }
}