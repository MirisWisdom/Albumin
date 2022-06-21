<?php

namespace Tests\Unit;

use App\Models\Time;
use PHPUnit\Framework\TestCase;

class TimeTest extends TestCase
{
    /**
     * Tests that 60 seconds == 60 seconds.
     *
     * @return void
     */
    public function test_seconds_in_seconds_is_correct()
    {
        $expected = '60';
        $actual   = Time::toSeconds('00:00:60');

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests that one minute == 60 seconds.
     *
     * @return void
     */
    public function test_minutes_in_seconds_is_correct_01()
    {
        $expected = '60';
        $actual   = Time::toSeconds('00:01:00');

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests that 18 minutes & 28 seconds == 1108 seconds.
     *
     * @return void
     */
    public function test_minutes_in_seconds_is_correct_02()
    {
        $expected = '1108';
        $actual   = Time::toSeconds('00:18:28');

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests that 8 hours, 18 minutes & 18 seconds == 29908 seconds.
     *
     * @return void
     */
    public function test_hours_in_seconds_is_correct()
    {
        $expected = '29908';
        $actual   = Time::toSeconds('08:18:28');

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests that 60 seconds == 1 minute (00:01:00).
     *
     * @return void
     */
    public function test_seconds_in_minutes_is_correct()
    {
        $expected = '00:01:00';
        $actual   = Time::fromSeconds(60);

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests that 60 seconds == 1 minute (00:01:00).
     *
     * @return void
     */
    public function test_seconds_in_hours_is_correct()
    {
        $expected = '08:18:28';
        $actual   = Time::fromSeconds(29908);

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests that passing data through both Time::fromSeconds() and Time::toSeconds() works reliably.
     *
     * @return void
     */
    public function test_time_integration_consistency()
    {
        $expected = '08:18:28';
        $actual   = Time::fromSeconds(Time::toSeconds($expected));

        $this->assertEquals($expected, $actual);
    }
}
