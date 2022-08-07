<?php

namespace App\Models;

class Time
{
    public static function toSeconds($time): float|int
    {
        $array = explode(':', $time);

        $hours   = (int)($array[0] ?? 0);
        $minutes = (int)($array[1] ?? 0);
        $seconds = (int)($array[2] ?? 0);

        return ($hours * 60 * 60) + ($minutes * 60) + ($seconds);
    }

    public static function fromSeconds($seconds): string
    {
        return sprintf('%02d:%02d:%02d', ($seconds / (60 * 60)), ($seconds / 60 % 60), $seconds % 60);
    }
}
