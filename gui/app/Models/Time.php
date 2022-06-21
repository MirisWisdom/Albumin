<?php

namespace App\Models;

class Time
{
    public static function toSeconds($time): float|int
    {
        $array = explode(':', $time);

        $hours   = (int)$array[0];
        $minutes = (int)$array[1];
        $seconds = (int)$array[2];

        return ($hours * 60 * 60) + ($minutes * 60) + ($seconds);
    }
}
