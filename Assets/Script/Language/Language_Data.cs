
public class Language_Data
{
    public static int select_language = 0;

    #region[공통 언어]
    public static string[] language_title_name = new string[]
    { "언어", "Language" };
    public static string[] language_name = new string[]
    { "한국어", "English" };

    public static string[] delete_name = new string[]
    {"삭제" , "Delete"};
    public static string[] create_name = new string[]
    {"생성" , "create"};
    public static string[] second_name = new string[]
    {"초" , "s"};

    public static string[] option_title_name = new string[]
    {"종류", "Type" };

    public static string[] option_setting_title = new string[]
    {"설정", "Setting"};

    #endregion

    #region[플랫폼 언어]

    public static string[] plaform_title_name = new string[]
    { "플랫폼 메이커", "Platform Maker" };
    public static string[] plaform_title_help_data = new string[]
    { "이 컴포넌트는 플랫폼 이벤트에 유용한 기능을 제공합니다.", "This component provides useful capabilities for platform events." };

    public static string[] option = new string[] { "", "", "" };
    public static string[] option_help = new string[] { "", "", "" };

    public static string[,] option_name = new string[,]
    {
        { "없음" , "None" },
        { "이동", "Move" },
        { "은신" , "Hide" }}
    ;
    public static string[,] option_help_data = new string[,]
    {
        { "플랫폼에 기능을 주지않습니다.", "It does not function on the platform." },
        { "플렛폼이 사용자가 지정한 경로로 움직입니다.", "The platform moves to the user-specified path." },
        { "플랫폼이 특정주기로 사라지는 효과를 얻습니다.", "The platform has the effect of disappearing into a specific cycle." }
    };

    public static string[] route_rotation_title = new string[]
    {"순환", "Rotate" };
    public static string[] route_rotation_title_help_data = new string[]
    { "경로가 순환하는 모양인지 설정합니다.", "Sets whether the path looks like it is rotating." };

    public static string[] point_data_title = new string[]
    {"점들", "Points" };
    public static string[] point_data_title_help_data = new string[]
    { "경로를 구성하는 점들의 좌표를 가지고 있습니다.", "They have coordinates of the points that make up the path." };

    public static string[] repeat_name = new string[]
    {"반복", "Repeat" };
    public static string[,] repeat_rotation_name_help_data = new string[,]
    {
        { "목표 지점까지 이동 후에 이동이 다시 시작되는지 설정합니다.", "Sets whether the movement starts again after moving to the target point." },
        { "순환형일 경우 자동으로 반복 실행됩니다.", "If circular, it will run automatically." }
    };

    public static string[] move_state_name = new string[]
    {"이동상태", "MoveState" };
    public static string[] move_state_help_data = new string[]
    {"슬라이더를 통해서 플랫폼의 위치를 조정할수있습니다.", "You can adjust the position of the platform through the slider." };

    public static string[] move_speed_name = new string[]
    {"이동속도", "MoveSpeed" };
    public static string[] move_speed_help_data = new string[]
    {"플랫폼의 이동속도입니다.", "The moving speed of the platform." };

    public static string[] route_cycle_name = new string[]
    {"총 시간(초)", "Total Time(s)" };
    public static string[] route_cycle_help_data = new string[]
    {"플랫폼이 하나의 애니메이션이 끝날때 까지의 시간을 의미합니다.", "The time until the platform is finished with one animation." };

    public static string[] now_platform_time_name = new string[]
    {"현재 플랫폼 시간(초)", "Now Platform Time(s)" };
    public static string[] now_platform_time_help_data = new string[]
    {"현재 플랫폼 애니메이션의 시간을 나타냅니다.", "Indicates the time of the current platform animation." };

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public static string[] collider_enabled_check_name = new string[]
    {"콜리더 활성화", "Collider Active" };
    public static string[] collider_enabled_check_help_data = new string[]
    {"은신할때마다 콜리더가 활성화되는지 설정합니다.", "Sets whether the collider is active whenever hiding." };

    public static string[] repeat_hide_name_help_data = new string[]
    {
         "애니메이션이 반복되는지 설정합니다.", "Sets whether the animation repeats."
    };

    public static string[] animation_option_title = new string[]
    {"애니메이션 옵션", "Animation Option" };
    public static string[] animation_option_help_data = new string[]
    {"애니메이션의 주기나 사라지는 속도 등을 조정합니다.", "Adjust the period of the animation or the speed at which it disappears." };

    public static string[] animate_time_name = new string[]
    {"애니메이션 시간(초)", "Animation Time(s)" };
    public static string[] animate_time_help_data = new string[]
    {"플랫폼이 사라지고 다시 나타나는데 까지의 애니메이션의 전체적인 시간을 설정합니다.", "Sets the overall time of the animation until the platform disappears and reappears." };

    public static string[] animate_enabled_time_name = new string[]
    {"나타나는 시간(초)", "enabled Time(s)" };
    public static string[] animate_enabled_time_help_data = new string[]
    {"플랫폼이 사라진 상태에서 나타난 상태로 바뀌는 시간을 설정합니다.", "Sets the amount of time the platform changes from a missing state to a visible state." };

    public static string[] animate_disenabled_time_name = new string[]
    {"사라지는 시간(초)", "disenabled Time(s)" };
    public static string[] animate_disenabled_time_help_data = new string[]
    {"플랫폼이 나타난 상태에서 사라진 상태로 바뀌는 시간을 설정합니다.", "Sets the time the platform changes from appearing to missing." };

    public static string[] hide_state_name = new string[]
    {"은신단계", "HideLevel" };
    public static string[] hide_state_help_data = new string[]
    {"슬라이더를 통해서 플랫폼의 은신상태를 조정할수있습니다.\n" +
     "슬라이더가 없는 부분은 플랫폼이 안보이는 부분이고\n" +
     "슬라이더가 있는 부분은 플랫폼이 보이는 부분입니다.",
     "You can adjust the hiding position of the platform through the slider.\n" +
     "The part without the slider is the part where you can't see the platform.\n" +
     "The part where the slider is located is where you see the platform." };

    public static string[] hide_start_name = new string[]
    {"은신하는 시간(초)", "Hide Time(s)" };

    public static string[] show_start_name = new string[]
    {"나타나는 시간(초)", "Show Time(s)" };

    public static string[] animate_state_name = new string[]
    {"애니메이션 상태 조절", "Animation Controler" };
    #endregion

    #region[플레이어 언어]
    public static string[] player_title_name = new string[]
    { "플레이어 메이커", "Player Maker" };
    public static string[] player_title_help_data = new string[]
    { "이 컴포넌트는 플레이어에게 유용한 기능을 제공합니다.", "This component provides useful features for the player." };

    public static string[] player_stat_name = new string[]
    { "플레이어 설정", "Player Setting" };

    public static string[] player_speed_name = new string[]
    { "플레이어 이동속도", "Player Speed" };
    public static string[] player_speed_help_data = new string[]
    { "플레이어의 이동속도를 조정할 수 있습니다.", "You can adjust the speed of the Player's movement." };

    public static string[] player_jump_name = new string[]
    { "플레이어 점프력", "Player Jump Power" };
    public static string[] player_jump_help_data = new string[]
    { "플레이어의 점프력을 설정합니다.", "Sets the player's jump force." };

    public static string[] double_jump_name = new string[]
    { "이단점프 여부", "Double Jump Check" };
    public static string[] double_jump_help_data = new string[]
    { "이단점프가 가능여부를 설정합니다.", "Sets whether a two-way jump is possible." };

    public static string[] player_hp_name = new string[]
    { "플레이어 체력", "Player Hp" };
    public static string[] player_hp_help_data = new string[]
    { "플레이어의 체력을 설정합니다.", "Set player hp." };
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////
    ////////////////////////////////////
    /// <summary>
    /// 
    /// </summary>
    /// 
    public static string[] value_setting_title = new string[]
    {"값", "Value"};


    public static string[] camera_title_name = new string[]
    { "카메라 메이커", "Camera Maker" };
    public static string[] camera_title_help_data = new string[]
    { "이 컴포넌트는 카메라를 조작하는 기능을 제공합니다.", "This component provides the ability to operate the camera." };
    public static string[] camera_point_data_title_help_data = new string[]
    { "카메라가 움직일수있는 곳을 지정하는 범위값을 가진 점들입니다.", "Points with range values that specify where the camera can move." };

    public static string[] camera_pivot_name = new string[]
    { "축", "Pivot" };
    public static string[] camera_pivot_help_data = new string[]
    { "카메라의 중심점의 좌표를 조정합니다.", "Adjust the coordinates of the center point of the camera." };

    public static string[] camera_goal_name = new string[]
    { "목표", "Gaol" };
    public static string[] camera_goal_help_data = new string[]
    { "카메라가 따라갈 오브젝트를 정합니다.(없을경우 움직이지 않습니다.)", "The camera decides which object to follow (if not, it won't move)." };

    public static string[] camera_speed_name = new string[]
    { "카메라 스피드", "Camera Speed" };
    public static string[] camera_speed_help_data = new string[]
    { "카메라가 오브젝트를 따라가는 속도를 설정합니다.", "Sets the speed at which the camera follows an object." };
}
